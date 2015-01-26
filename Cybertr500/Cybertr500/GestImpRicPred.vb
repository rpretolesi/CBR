Imports System.Data.SqlClient

Public Class GestImpRicPred

    Private m_bStopSalvaInSQL As Boolean

    Private Sub GestImpRicPred_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Text = Main.APP_TITLE
        Me.Icon = Drawing.Icon.ExtractAssociatedIcon(My.Application.Info.DirectoryPath.ToString() + "\Icon.ico")

        'TODO: questa riga di codice carica i dati nella tabella 'DataSet_RicPrdsg.RicPrdsg'. È possibile spostarla o rimuoverla se necessario.
        Try
            Dim str As String

            str = Me.RicPrdsgTableAdapter.Connection.ConnectionString()
            str = str + " - " + Me.RicPrdsgTableAdapter.Connection.Database()
            str = str + " - " + Me.RicPrdsgTableAdapter.Connection.DataSource()
            Me.Text = Me.Text + " - " + str

            Me.RicPrdsgTableAdapter.Fill(Me.DataSet_RicPrdsg.RicPrdsg)
        Catch ex As Exception
            AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
            System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            System.Windows.Forms.MessageBox.Show("Il File selezionato non rappresenta le ricette di predosaggio", Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
        End Try

        RicPrdsgBindingNavigatorSaveItem.Enabled = True
        RicPrdsgNomiProdBindingNavigatorSaveItem.Enabled = True
        RicPrdsgBindingNavigatorStopSaveItem.Enabled = False

        ToolStripLabel_Avanzamento.Text = "Stato avanzamento importazione: 0 - " + Me.DataSet_RicPrdsg.RicPrdsg.Rows.Count.ToString() + " - Record Inseriti: 0"
    End Sub

    Private Sub GestImpRicPred_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        m_bStopSalvaInSQL = True
    End Sub

    Private Sub RicPrdsgBindingNavigatorSaveItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RicPrdsgBindingNavigatorSaveItem.Click
        AddLogEvent(LOG_OK, "Esecuzione Salva Ricette Di Predosaggio.", Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")

        RicPrdsgBindingNavigatorSaveItem.Enabled = False
        RicPrdsgNomiProdBindingNavigatorSaveItem.Enabled = False
        RicPrdsgBindingNavigatorStopSaveItem.Enabled = True

        Try
            ' A questo punto chiedo conferma per l'esportazione
            ' Quindi importo il file
            Dim iTotalRecordRecipe As Integer
            Dim iActualRecordRecipe As Integer
            Dim iActualInsertedRecordRecipe As Integer
            Dim bImportRes As Boolean
            Dim strSQL, strConn As String
            Dim strDataIntest As String
            strDataIntest = Date.Now.ToString

            Dim ds As New DataSet
            Dim iRPDI_ID As Integer
            Dim iIndice_1 As Integer

            Dim strStringTemp As String
            Dim fFloatTemp As Single
            Dim iIntTemp As Integer

            Dim bIntestRicettaTrovata As Boolean
            Dim bIntestRicettaTrovataGiaEsistente As Boolean
            Dim bRicettaImportata As Boolean
            Dim drDR As Windows.Forms.DialogResult
            Dim bAskForDR As Boolean

            strConn = My.Settings.Item("db_ConnectionString")
            Dim cn As New SqlConnection(strConn)
            Dim txn As SqlTransaction
            Dim cmd As New SqlCommand
            cn.Open()
            cmd.Connection = cn

            ' Lo imposto inizialmente a True
            bImportRes = True
            iTotalRecordRecipe = Me.DataSet_RicPrdsg.RicPrdsg.Rows.Count
            iActualRecordRecipe = 0
            iActualInsertedRecordRecipe = 0
            m_bStopSalvaInSQL = False
            bIntestRicettaTrovata = False
            bIntestRicettaTrovataGiaEsistente = False
            bAskForDR = False
            For Each dr As DataRow In Me.DataSet_RicPrdsg.RicPrdsg.Rows
                ' Verifico che il file non sia gia' stato importato confrontando il nome della ricetta
                If Not dr.Item("NRPrdsg") Is DBNull.Value Then
                    If dr.Item("NRPrdsg") <> "" Then
                        ' Do il commit per l'importazione precedente
                        Try
                            If Not txn Is Nothing Then
                                txn.Commit()
                            End If
                        Catch ex As System.Exception
                        End Try
                        strSQL = "SELECT RPDI_Nome FROM RicPreDosInt WHERE RPDI_Nome = '" + dr.Item("NRPrdsg") + "'"
                        ApriDataSet(strSQL, "", ds, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                        If ds.Tables(0).Rows.Count > 0 Then
                            bIntestRicettaTrovata = False
                            bIntestRicettaTrovataGiaEsistente = False
                            AddLogEvent(LOG_WARNING, "Attenzione, la ricetta: " + dr.Item("NRPrdsg") + " e' gia' presente", Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")

                            If bAskForDR = False Then
                                drDR = System.Windows.Forms.MessageBox.Show("Attenzione, la ricetta: " + dr.Item("NRPrdsg") + " e' gia' presente, sostituirla ?", Main.APP_TITLE, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning)
                                If drDR <> Windows.Forms.DialogResult.Cancel Then
                                    If System.Windows.Forms.MessageBox.Show("Confermi la risposta anche per le occorrenze successive ?", Main.APP_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                        bAskForDR = True
                                    End If
                                End If
                            End If

                            If drDR = Windows.Forms.DialogResult.Yes Then
                                AddLogEvent(LOG_WARNING, "Si, la ricetta: " + dr.Item("NRPrdsg") + " sara' sostituita", Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                                bIntestRicettaTrovataGiaEsistente = True
                            ElseIf drDR = Windows.Forms.DialogResult.Cancel Then
                                bImportRes = False
                                Exit For
                            End If
                            bRicettaImportata = False
                        Else
                            bIntestRicettaTrovata = True
                            bIntestRicettaTrovataGiaEsistente = False
                            bRicettaImportata = False
                        End If

                        ' Se do lo stop, finisco cmq l'importazione dei dati di quella produzione
                        If m_bStopSalvaInSQL = True Then
                            bImportRes = False
                            Exit For
                        End If
                    End If
                End If

                If bIntestRicettaTrovataGiaEsistente = True Then

                    ' Prima la elimino
                    strSQL = "SELECT RPDI_ID FROM RicPreDosInt WHERE RPDI_Nome = '" + dr.Item("NRPrdsg") + "'"
                    ApriDataSet(strSQL, "", ds, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                    iRPDI_ID = ds.Tables(0).Rows(0).Item("RPDI_ID")

                    ' setto la transazione
                    txn = cn.BeginTransaction
                    cmd.Transaction = txn

                    strSQL = "DELETE [RicPreDosPassi] "
                    strSQL = strSQL + "WHERE RPDP_RPDI_ID = @RPDP_RPDI_ID"

                    'cmd.Transaction = txn
                    'cmd.Connection = cn
                    cmd.CommandText = strSQL

                    cmd.Parameters.AddWithValue("@RPDP_RPDI_ID", iRPDI_ID)

                    Try
                        If cmd.ExecuteNonQuery() > 0 Then
                            bIntestRicettaTrovata = True
                            'System.Windows.Forms.MessageBox.Show("Eliminazione dati OK", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Else
                            If Not txn Is Nothing Then
                                txn.Rollback()
                            End If
                            bImportRes = False

                            AddLogEvent(LOG_ERROR, "Errore nell'Eliminazione dei Passi della Ricetta.", Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                            System.Windows.Forms.MessageBox.Show("Errore nell'Eliminazione dei Passi della Ricetta.", Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                            Exit For
                        End If
                    Catch ex As Exception
                        If Not txn Is Nothing Then
                            txn.Rollback()
                        End If
                        bImportRes = False
                        AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                        System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                        Exit For
                    End Try

                    cmd.Parameters.Clear()

                    strSQL = "DELETE [RicPreDosInt] "
                    strSQL = strSQL + "WHERE RPDI_ID = @RPDI_ID"

                    'cmd.Transaction = txn
                    'cmd.Connection = cn
                    cmd.CommandText = strSQL

                    cmd.Parameters.AddWithValue("@RPDI_ID", iRPDI_ID)

                    Try
                        If cmd.ExecuteNonQuery() > 0 Then
                            bIntestRicettaTrovata = True
                            'System.Windows.Forms.MessageBox.Show("Eliminazione dati OK", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Else
                            If Not txn Is Nothing Then
                                txn.Rollback()
                            End If
                            bImportRes = False
                            AddLogEvent(LOG_ERROR, "Errore nell'Eliminazione dell'Intestazione della Ricetta.", Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                            System.Windows.Forms.MessageBox.Show("Errore nell'Eliminazione dell'Intestazione della Ricetta.", Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                            Exit For
                        End If
                    Catch ex As Exception
                        If Not txn Is Nothing Then
                            txn.Rollback()
                        End If
                        bImportRes = False
                        AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                        System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                        Exit For
                    End Try

                    txn.Commit()

                    cmd.Parameters.Clear()
                End If

                If bIntestRicettaTrovata = True Then

                    ' Inserisco l'intestazione della ricetta
                    ' Genero l'ID Dell'intestazione ricetta
                    strSQL = "SELECT MAX(RPDI_ID) AS MASSIMO FROM RicPreDosInt"
                    ApriDataSet(strSQL, "", ds, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                    If Not ds.Tables(0).Rows(0).Item("MASSIMO") Is DBNull.Value Then
                        iRPDI_ID = ds.Tables(0).Rows(0).Item("MASSIMO")
                    Else
                        iRPDI_ID = 1
                    End If

                    ' setto la transazione
                    txn = cn.BeginTransaction
                    cmd.Transaction = txn

                    iRPDI_ID = iRPDI_ID + 1
                    strSQL = "INSERT INTO [RicPreDosInt] "
                    strSQL = strSQL + "  (RPDI_ID,  RPDI_Nome,  RPDI_Descrizione,  RPDI_PredRicInV,  RPDI_PrRicSeEl,  RPDI_EsclBrImp,  RPDI_TmpRicPrd,  RPDI_MRSuAnEdEl) VALUES "
                    strSQL = strSQL + "  (@RPDI_ID, @RPDI_Nome, @RPDI_Descrizione, @RPDI_PredRicInV, @RPDI_PrRicSeEl, @RPDI_EsclBrImp, @RPDI_TmpRicPrd, @RPDI_MRSuAnEdEl)"

                    'cmd.Transaction = txn
                    'cmd.Connection = cn
                    cmd.CommandText = strSQL

                    cmd.Parameters.AddWithValue("@RPDI_ID", iRPDI_ID)
                    cmd.Parameters.AddWithValue("@RPDI_Nome", dr.Item("NRPrdsg"))
                    cmd.Parameters.AddWithValue("@RPDI_Descrizione", dr.Item("DRPrdsg"))

                    Try
                        strStringTemp = String.Format(dr.Item("PredRicInV").ToString, "###").Replace(",00", "")
                        iIntTemp = System.Convert.ToInt32(strStringTemp)
                    Catch ex As System.Exception
                        iIntTemp = 0
                    End Try
                    cmd.Parameters.AddWithValue("@RPDI_PredRicInV", iIntTemp)

                    Try
                        strStringTemp = String.Format(dr.Item("PrRicSeEl").ToString, "###").Replace(",00", "")
                        iIntTemp = System.Convert.ToInt32(strStringTemp)
                    Catch ex As System.Exception
                        iIntTemp = 0
                    End Try
                    cmd.Parameters.AddWithValue("@RPDI_PrRicSeEl", iIntTemp)

                    Try
                        strStringTemp = String.Format(dr.Item("EsclBrImp").ToString, "###").Replace(",00", "")
                        iIntTemp = System.Convert.ToInt32(strStringTemp)
                    Catch ex As System.Exception
                        iIntTemp = 0
                    End Try
                    cmd.Parameters.AddWithValue("@RPDI_EsclBrImp", iIntTemp)

                    Try
                        strStringTemp = String.Format(dr.Item("TmpRicPrd").ToString, "###").Replace("*C", "")
                        iIntTemp = System.Convert.ToInt32(strStringTemp)
                    Catch ex As System.Exception
                        iIntTemp = 0
                    End Try
                    cmd.Parameters.AddWithValue("@RPDI_TmpRicPrd", iIntTemp)

                    Try
                        strStringTemp = String.Format(dr.Item("MRSuAnEdE").ToString, "###").Replace(",00", "")
                        iIntTemp = System.Convert.ToInt32(strStringTemp)
                    Catch ex As System.Exception
                        iIntTemp = 0
                    End Try
                    cmd.Parameters.AddWithValue("@RPDI_MRSuAnEdEl", iIntTemp)

                    Try
                        If cmd.ExecuteNonQuery() > 0 Then
                            'System.Windows.Forms.MessageBox.Show("Inserimento dati OK", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Else
                            If Not txn Is Nothing Then
                                txn.Rollback()
                            End If
                            bImportRes = False

                            AddLogEvent(LOG_ERROR, "Errore nell'Inserimento Intestazione Ricetta.", Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                            System.Windows.Forms.MessageBox.Show("Errore nell'Inserimento Intestazione Ricetta.", Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                            Exit For
                        End If
                    Catch ex As Exception
                        If Not txn Is Nothing Then
                            txn.Rollback()
                        End If
                        bImportRes = False
                        AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                        System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                        Exit For
                    End Try

                    cmd.Parameters.Clear()

                    ' Passi ricetta
                    For iIndice_1 = 0 To 19
                        strSQL = "INSERT INTO [RicPreDosPassi] "
                        strSQL = strSQL + "  (RPDP_RPDI_ID,  RPDP_Tramoggia, RPDP_Nome_Prodotto,  RPDP_Set_Perc,  RPDP_Set_Toll,  RPDP_Set_Rit_Start,  RPDP_Set_Rit_Stop,  RPDP_Set_Ponderale) VALUES "
                        strSQL = strSQL + "  (@RPDP_RPDI_ID, @RPDP_Tramoggia, @RPDP_Nome_Prodotto, @RPDP_Set_Perc, @RPDP_Set_Toll, @RPDP_Set_Rit_Start, @RPDP_Set_Rit_Stop, @RPDP_Set_Ponderale)"

                        'cmd.Transaction = txn
                        'cmd.Connection = cn
                        cmd.CommandText = strSQL

                        cmd.Parameters.AddWithValue("@RPDP_RPDI_ID", iRPDI_ID)
                        cmd.Parameters.AddWithValue("@RPDP_Tramoggia", dr.Table.Columns(iIndice_1 + 82).Caption.ToString)
                        cmd.Parameters.AddWithValue("@RPDP_Nome_Prodotto", dr.Item(iIndice_1 + 82).ToString)

                        Try
                            strStringTemp = String.Format(dr.Item(iIndice_1 + 2).ToString, "0.###").Replace("%", "")
                            strStringTemp = String.Format(strStringTemp, "0.###").Replace(", ", ".")
                            fFloatTemp = System.Convert.ToSingle(strStringTemp)
                        Catch ex As System.Exception
                            fFloatTemp = 0
                        End Try
                        cmd.Parameters.AddWithValue("@RPDP_Set_Perc", fFloatTemp)

                        Try
                            strStringTemp = String.Format(dr.Item(iIndice_1 + 22).ToString, "0.###").Replace("%", "")
                            strStringTemp = String.Format(strStringTemp, "0.###").Replace(", ", ".")
                            fFloatTemp = System.Convert.ToSingle(strStringTemp)
                        Catch ex As System.Exception
                            fFloatTemp = 0
                        End Try
                        cmd.Parameters.AddWithValue("@RPDP_Set_Toll", fFloatTemp)

                        Try
                            strStringTemp = String.Format(dr.Item(iIndice_1 + 42).ToString, "###").Replace("Sec.", "")
                            iIntTemp = System.Convert.ToSingle(strStringTemp)
                        Catch ex As System.Exception
                            iIntTemp = 0
                        End Try
                        cmd.Parameters.AddWithValue("@RPDP_Set_Rit_Start", iIntTemp)

                        Try
                            strStringTemp = String.Format(dr.Item(iIndice_1 + 62).ToString, "###").Replace("Sec.", "")
                            iIntTemp = System.Convert.ToSingle(strStringTemp)
                        Catch ex As System.Exception
                            iIntTemp = 0
                        End Try
                        cmd.Parameters.AddWithValue("@RPDP_Set_Rit_Stop", iIntTemp)

                        Try
                            strStringTemp = String.Format(dr.Item(iIndice_1 + 102).ToString, "###").Replace(",00", "")
                            iIntTemp = System.Convert.ToInt32(strStringTemp)
                        Catch ex As System.Exception
                            iIntTemp = 0
                        End Try
                        cmd.Parameters.AddWithValue("@RPDP_Set_Ponderale", iIntTemp)

                        Try
                            If cmd.ExecuteNonQuery() > 0 Then
                                'System.Windows.Forms.MessageBox.Show("Inserimento dati OK", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Else
                                If Not txn Is Nothing Then
                                    txn.Rollback()
                                End If
                                bImportRes = False

                                AddLogEvent(LOG_ERROR, "Errore nell'Inserimento Passi Ricetta.", Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                                System.Windows.Forms.MessageBox.Show("Errore nell'Inserimento Passi Ricetta.", Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                Exit For
                            End If
                        Catch ex As Exception
                            If Not txn Is Nothing Then
                                txn.Rollback()
                            End If
                            bImportRes = False
                            AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                            System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                            Exit For
                        End Try

                        cmd.Parameters.Clear()

                    Next iIndice_1

                    iActualInsertedRecordRecipe = iActualInsertedRecordRecipe + 1
                End If

                ' Visualizzo l'avanzamento
                iActualRecordRecipe = iActualRecordRecipe + 1
                ToolStripLabel_Avanzamento.Text = "Stato avanzamento importazione Record: " + iActualRecordRecipe.ToString + " - " + iTotalRecordRecipe.ToString + " - Record Inseriti: " + iActualInsertedRecordRecipe.ToString
                My.Application.DoEvents()

            Next dr

            If bImportRes = True Then
                Try
                    If Not txn Is Nothing Then
                        txn.Commit()
                    End If
                    AddLogEvent(LOG_OK, "Salvataggio Ricette Di Predosaggio OK.", Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                    System.Windows.Forms.MessageBox.Show("Salvataggio Ricette Di Predosaggio OK.", Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Catch ex As System.Exception
                    AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                    AddLogEvent(LOG_ERROR, "Errore nel Commit Ricette Di Predosaggio.", Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                    System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                    System.Windows.Forms.MessageBox.Show("Errore nel Commit Ricette Di Predosaggio.", Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                End Try
            End If
            cn.Close()

        Catch ex As Exception
            System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, "Cooperativa Braccianti Riminese", MessageBoxButtons.OK, MessageBoxIcon.Stop)
        End Try

        RicPrdsgBindingNavigatorSaveItem.Enabled = True
        RicPrdsgNomiProdBindingNavigatorSaveItem.Enabled = True
        RicPrdsgBindingNavigatorStopSaveItem.Enabled = False

    End Sub

    Private Sub RicPrdsgNomiProdBindingNavigatorSaveItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RicPrdsgNomiProdBindingNavigatorSaveItem.Click
        AddLogEvent(LOG_OK, "Esecuzione Salva Nomi Prodotti Preosaggio.", Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")

        RicPrdsgBindingNavigatorSaveItem.Enabled = False
        RicPrdsgNomiProdBindingNavigatorSaveItem.Enabled = False
        RicPrdsgBindingNavigatorStopSaveItem.Enabled = True

        Try
            ' A questo punto chiedo conferma per l'esportazione
            ' Quindi importo il file
            Dim iTotalRecordRecipe As Integer
            Dim iActualRecordRecipe As Integer
            Dim iActualInsertedRecordRecipe As Integer
            Dim iActualJobRecipe As Integer
            Dim bImportRes As Boolean
            Dim strSQL, strConn As String
            Dim strDataIntest As String
            strDataIntest = Date.Now.ToString

            Dim ds As New DataSet
            Dim iIndice_1 As Integer

            strConn = My.Settings.Item("db_ConnectionString")
            Dim cn As New SqlConnection(strConn)
            Dim cmd As New SqlCommand
            cn.Open()
            cmd.Connection = cn

            ' Lo imposto inizialmente a True
            bImportRes = True
            iTotalRecordRecipe = Me.DataSet_RicPrdsg.RicPrdsg.Rows.Count
            iActualRecordRecipe = 0
            iActualInsertedRecordRecipe = 0
            iActualJobRecipe = 0
            m_bStopSalvaInSQL = False
            For Each dr As DataRow In Me.DataSet_RicPrdsg.RicPrdsg.Rows
                ' Importo solo i nomi dei prodotti dalla ricetta di predosaggio
                For iIndice_1 = 0 To 19
                    If Not dr.Item(iIndice_1 + 82) Is DBNull.Value Then
                        If dr.Item(iIndice_1 + 82).ToString <> "" Then
                            ' Verifico che non esista gia'
                            strSQL = "SELECT NPPD_Tramoggia, NPPD_Nome_Prodotto FROM NomiProdottiPreDosaggio WHERE NPPD_Tramoggia = '" + dr.Table.Columns(iIndice_1 + 82).ColumnName.ToString + "' AND NPPD_Nome_Prodotto ='" + dr.Item(iIndice_1 + 82).ToString + "'"
                            ApriDataSet(strSQL, "", ds, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                            If ds.Tables(0).Rows.Count > 0 Then
                                ' Esiste gia', non faccio niente
                            Else
                                ' Non Esiste, la inserisco
                                strSQL = "INSERT INTO [NomiProdottiPreDosaggio] "
                                strSQL = strSQL + "  (NPPD_Tramoggia,  NPPD_Nome_Prodotto) VALUES "
                                strSQL = strSQL + "  (@NPPD_Tramoggia, @NPPD_Nome_Prodotto)"

                                cmd.CommandText = strSQL

                                cmd.Parameters.AddWithValue("@NPPD_Tramoggia", dr.Table.Columns(iIndice_1 + 82).ColumnName.ToString())
                                cmd.Parameters.AddWithValue("@NPPD_Nome_Prodotto", dr.Item(iIndice_1 + 82).ToString())

                                Try
                                    If cmd.ExecuteNonQuery() > 0 Then
                                        'System.Windows.Forms.MessageBox.Show("Inserimento dati OK", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                    Else
                                        bImportRes = False
                                        AddLogEvent(LOG_ERROR, "Errore nell'Inserimento dei dati.", Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                                        System.Windows.Forms.MessageBox.Show("Errore nell'Inserimento dei dati.", Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                        Exit For
                                    End If
                                Catch ex As Exception
                                    bImportRes = False
                                    AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                                    System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                    Exit For
                                End Try

                                cmd.Parameters.Clear()

                                iActualInsertedRecordRecipe = iActualInsertedRecordRecipe + 1

                            End If
                        End If
                    End If
                Next

                ' Visualizzo l'avanzamento
                iActualRecordRecipe = iActualRecordRecipe + 1
                ToolStripLabel_Avanzamento.Text = "Stato avanzamento importazione Record: " + iActualRecordRecipe.ToString + " - " + iTotalRecordRecipe.ToString + " - Job:" + iActualJobRecipe.ToString + " - Record Inseriti: " + iActualInsertedRecordRecipe.ToString
                My.Application.DoEvents()

            Next dr

            If bImportRes = True Then
                AddLogEvent(LOG_OK, "Salvataggio Nomi Prodotti Predosaggio OK.", Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                System.Windows.Forms.MessageBox.Show("Salvataggio Nomi Prodotti Predosaggio OK.", Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            cn.Close()

        Catch ex As Exception
            AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
            System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
        End Try

        RicPrdsgBindingNavigatorSaveItem.Enabled = True
        RicPrdsgNomiProdBindingNavigatorSaveItem.Enabled = True
        RicPrdsgBindingNavigatorStopSaveItem.Enabled = False

    End Sub

    Private Sub RicPrdsgBindingNavigatorStopSaveItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RicPrdsgBindingNavigatorStopSaveItem.Click
        m_bStopSalvaInSQL = True
        AddLogEvent(LOG_WARNING, "Operazione Interrotta Dall'Utente.", Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
        System.Windows.Forms.MessageBox.Show("Operazione Interrotta Dall'Utente.", Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        RicPrdsgBindingNavigatorSaveItem.Enabled = False
        RicPrdsgNomiProdBindingNavigatorSaveItem.Enabled = False
        RicPrdsgNomiProdBindingNavigatorSaveItem.Enabled = False
    End Sub

End Class