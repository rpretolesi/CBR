Imports System.Data.SqlClient

Public Class GestImpProd

    Private m_bStopSalvaInSQL As Boolean

    Private Sub GestEspInSQL_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Text = Main.APP_TITLE
        Me.Icon = Drawing.Icon.ExtractAssociatedIcon(My.Application.Info.DirectoryPath.ToString() + "\Icon.ico")

        'TODO: questa riga di codice carica i dati nella tabella 'DataSet_Prod500.Prod500'. È possibile spostarla o rimuoverla se necessario.
        Try
            Dim str As String

            str = Me.Prod500TableAdapter.Connection.ConnectionString()
            str = str + " - " + Me.Prod500TableAdapter.Connection.Database()
            str = str + " - " + Me.Prod500TableAdapter.Connection.DataSource()
            Me.Text = Me.Text + " - " + str

            Me.Prod500TableAdapter.Fill(Me.DataSet_Prod500.Prod500)
        Catch ex As Exception
            AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
            System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            System.Windows.Forms.MessageBox.Show("Il File selezionato non rappresenta la produzione", Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
        End Try

        Prod500BindingNavigatorSaveItem.Enabled = True
        RicDosNomiProdBindingNavigatorSaveItem.Enabled = True
        Prod500BindingNavigatorStopSaveItem.Enabled = False

        ToolStripLabel_Avanzamento.Text = "Stato avanzamento importazione: 0 - " + Me.DataSet_Prod500.Prod500.Rows.Count.ToString() + " - Job: 0 - Record Inseriti: 0"
    End Sub

    Private Sub GestImpProd_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        m_bStopSalvaInSQL = True
    End Sub

    Private Sub Prod500BindingNavigatorSaveItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Prod500BindingNavigatorSaveItem.Click
        AddLogEvent(LOG_OK, "Esecuzione Salva Dati Di Produzione.", Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")

        Prod500BindingNavigatorSaveItem.Enabled = False
        RicDosNomiProdBindingNavigatorSaveItem.Enabled = False
        Prod500BindingNavigatorStopSaveItem.Enabled = True
        Try
            ' A questo punto chiedo conferma per l'esportazione
            ' Quindi importo il file
            Dim iTotalRecordRecipe As Integer
            Dim iActualRecordRecipe As Integer
            Dim iActualInsertedRecordRecipe As Integer
            Dim iActualJobRecipe As Integer
            Dim bCheckFileAlreadyImported As Boolean
            Dim bImportRes As Boolean
            Dim strSQL, strConn As String
            Dim strDataIntest As String
            strDataIntest = Date.Now.ToString

            Dim ds As New DataSet
            Dim dt As Date
            Dim iRIDI_ID As Integer
            Dim iRDI_ID As Integer
            Dim iIndice_1 As Integer

            Dim strStringTemp As String
            Dim fFloatTemp As Single

            Dim bIntestRicettaTrovata As Boolean
            Dim bRicettaImportata As Boolean

            strConn = My.Settings.Item("db_ConnectionString")
            Dim cn As New SqlConnection(strConn)
            Dim txn As SqlTransaction
            Dim cmd As New SqlCommand
            cn.Open()
            cmd.Connection = cn

            ' Lo imposto inizialmente a True
            bImportRes = True
            iTotalRecordRecipe = Me.DataSet_Prod500.Prod500.Rows.Count
            iActualRecordRecipe = 0
            iActualInsertedRecordRecipe = 0
            iActualJobRecipe = 0
            bCheckFileAlreadyImported = False
            m_bStopSalvaInSQL = False
            bIntestRicettaTrovata = False
            For Each dr As DataRow In Me.DataSet_Prod500.Prod500.Rows
                ' Verifico che il file non sia gia' stato importato confrontando la data e l'ora del primo record(intestazione)
                ' da importare, con quelli gia' presenti nel database
                If Not dr.Item("DataStart") Is DBNull.Value Then
                    If dr.Item("DataStart") <> "" Then
                        ' Do il commit per l'importazione precedente
                        Try
                            If Not txn Is Nothing Then
                                txn.Commit()
                                txn.Dispose()
                                txn = Nothing
                            End If
                        Catch ex As System.Exception
                        End Try
                        iActualJobRecipe = iActualJobRecipe + 1
                        strDataIntest = dr.Item("DataStart").ToString()
                        dt = Date.Parse(dr.Item("DataStart").ToString() + " " + dr.Item("OraStart").ToString())
                        strSQL = "SELECT RIDI_Data_Ora_Start FROM RicDosInt WHERE RIDI_Data_Ora_Start = '" + dt.ToString("G", Globalization.CultureInfo.CreateSpecificCulture("de-DE")) + "'" 'DateTimePicker_Inizio.Value.ToString("G", Globalization.CultureInfo.CreateSpecificCulture("de-DE"))"
                        ApriDataSet(strSQL, "", ds, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                        If ds.Tables(0).Rows.Count > 0 Then
                            bIntestRicettaTrovata = False
                            bRicettaImportata = False
                        Else
                            bIntestRicettaTrovata = True
                            bRicettaImportata = False
                        End If

                        ' Se do lo stop, finisco cmq l'importazione dei dati di quella produzione
                        If m_bStopSalvaInSQL = True Then
                            bImportRes = False
                            Exit For
                        End If
                    End If
                End If

                If bIntestRicettaTrovata = True Then

                    If bRicettaImportata = False Then
                        bRicettaImportata = True

                        ' Inserisco l'intestazione della ricetta
                        ' Genero l'ID Dell'intestazione ricetta
                        strSQL = "SELECT MAX(RIDI_ID) AS MASSIMO FROM RicDosInt"
                        ApriDataSet(strSQL, "", ds, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                        If Not ds.Tables(0).Rows(0).Item("MASSIMO") Is DBNull.Value Then
                            iRIDI_ID = ds.Tables(0).Rows(0).Item("MASSIMO")
                        Else
                            iRIDI_ID = 1
                        End If
                        ' Genero l'ID Dell'intestazione report
                        strSQL = "SELECT MAX(RDI_ID) AS MASSIMO FROM RepDosInt"
                        ApriDataSet(strSQL, "", ds, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                        If Not ds.Tables(0).Rows(0).Item("MASSIMO") Is DBNull.Value Then
                            iRDI_ID = ds.Tables(0).Rows(0).Item("MASSIMO")
                        Else
                            iRDI_ID = 1
                        End If

                        ' setto la transazione
                        txn = cn.BeginTransaction
                        cmd.Transaction = txn

                        iRIDI_ID = iRIDI_ID + 1
                        strSQL = "INSERT INTO [RicDosInt] "
                        strSQL = strSQL + "  (RIDI_ID,  RIDI_Nome_Operatore,  RIDI_Nome_Ricetta,  RIDI_Nome_Ricetta_Predosaggio,  RIDI_Job_Descrizione,  RIDI_Data_Ora_Start,  RIDI_Silo_Destinazione,  RIDI_Rapporto_F_L) VALUES "
                        strSQL = strSQL + "  (@RIDI_ID, @RIDI_Nome_Operatore, @RIDI_Nome_Ricetta, @RIDI_Nome_Ricetta_Predosaggio, @RIDI_Job_Descrizione, @RIDI_Data_Ora_Start, @RIDI_Silo_Destinazione, @RIDI_Rapporto_F_L)"

                        'cmd.Transaction = txn
                        'cmd.Connection = cn
                        cmd.CommandText = strSQL

                        cmd.Parameters.AddWithValue("@RIDI_ID", iRIDI_ID)
                        cmd.Parameters.AddWithValue("@RIDI_Nome_Operatore", dr.Item("NomeOperat"))
                        cmd.Parameters.AddWithValue("@RIDI_Nome_Ricetta", dr.Item("NomeRic"))
                        cmd.Parameters.AddWithValue("@RIDI_Nome_Ricetta_Predosaggio", dr.Item("NomeRicPrd"))
                        cmd.Parameters.AddWithValue("@RIDI_Job_Descrizione", dr.Item("JobDescr"))

                        strDataIntest = dr.Item("DataStart").ToString()
                        dt = Date.Parse(dr.Item("DataStart").ToString() + " " + dr.Item("OraStart").ToString())
                        cmd.Parameters.AddWithValue("@RIDI_Data_Ora_Start", dt)

                        cmd.Parameters.AddWithValue("@RIDI_Silo_Destinazione", dr.Item("SiloDest"))
                        Try
                            strStringTemp = dr.Item("RappF_L").ToString
                            fFloatTemp = System.Convert.ToSingle(strStringTemp)
                        Catch ex As System.Exception
                            fFloatTemp = -1.0
                        End Try
                        cmd.Parameters.AddWithValue("@RIDI_Rapporto_F_L", String.Format(fFloatTemp, "0.###").Replace(",", "."))
                        Try
                            If cmd.ExecuteNonQuery() > 0 Then
                                'System.Windows.Forms.MessageBox.Show("Inserimento dati OK", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Else
                                If Not txn Is Nothing Then
                                    txn.Rollback()
                                    txn.Dispose()
                                    txn = Nothing
                                End If
                                bImportRes = False

                                AddLogEvent(LOG_ERROR, "Errore nell'Inserimento Intestazione Ricetta.", Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                                System.Windows.Forms.MessageBox.Show("Errore nell'Inserimento Intestazione Ricetta.", Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                Exit For
                            End If
                        Catch ex As Exception
                            If Not txn Is Nothing Then
                                txn.Rollback()
                                txn.Dispose()
                                txn = Nothing
                            End If
                            bImportRes = False
                            AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                            System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                            Exit For
                        End Try

                        cmd.Parameters.Clear()

                        ' Passi ricetta
                        For iIndice_1 = 0 To 19
                            strSQL = "INSERT INTO [RicDosPassi] "
                            strSQL = strSQL + "  (RIDP_RIDI_ID,  RIDP_NPD_Riferimento,  RIDP_Set_Kg_Prod,  RIDP_Set_Perc_Prod,  RIDP_Set_Toll_Prod) VALUES "
                            strSQL = strSQL + "  (@RIDP_RIDI_ID, @RIDP_NPD_Riferimento, @RIDP_Set_Kg_Prod, @RIDP_Set_Perc_Prod, @RIDP_Set_Toll_Prod)"

                            'cmd.Transaction = txn
                            'cmd.Connection = cn
                            cmd.CommandText = strSQL

                            cmd.Parameters.AddWithValue("@RIDP_RIDI_ID", iRIDI_ID)
                            cmd.Parameters.AddWithValue("@RIDP_NPD_Riferimento", dr.Table.Columns(iIndice_1 + 7).Caption.ToString)

                            Try
                                strStringTemp = dr.Item(iIndice_1 + 7).ToString
                                fFloatTemp = System.Convert.ToSingle(strStringTemp)
                            Catch ex As System.Exception
                                fFloatTemp = 0
                            End Try
                            cmd.Parameters.AddWithValue("@RIDP_Set_Kg_Prod", String.Format(fFloatTemp, "0.###").Replace(",", "."))

                            ' Aggregati e Filler
                            If iIndice_1 <= 10 Then
                                Try
                                    strStringTemp = dr.Item(iIndice_1 + 28).ToString
                                    fFloatTemp = System.Convert.ToSingle(strStringTemp)
                                Catch ex As System.Exception
                                    fFloatTemp = 0
                                End Try
                            Else
                                fFloatTemp = 0
                            End If
                            ' Legante
                            If iIndice_1 >= 11 And iIndice_1 <= 14 Then
                                Try
                                    strStringTemp = dr.Item(iIndice_1 + 29).ToString
                                    fFloatTemp = System.Convert.ToSingle(strStringTemp)
                                Catch ex As System.Exception
                                    fFloatTemp = 0
                                End Try
                            End If
                            ' Viatop
                            If iIndice_1 = 15 Then
                                Try
                                    strStringTemp = dr.Item(iIndice_1 + 24).ToString
                                    fFloatTemp = System.Convert.ToSingle(strStringTemp)
                                Catch ex As System.Exception
                                    fFloatTemp = 0
                                End Try
                            End If
                            ' Additivi
                            If iIndice_1 >= 16 And iIndice_1 <= 19 Then
                                Try
                                    strStringTemp = dr.Item(iIndice_1 + 28).ToString
                                    fFloatTemp = System.Convert.ToSingle(strStringTemp)
                                Catch ex As System.Exception
                                    fFloatTemp = 0
                                End Try
                            End If
                            cmd.Parameters.AddWithValue("@RIDP_Set_Perc_Prod", String.Format(fFloatTemp, "0.###").Replace(",", "."))

                            ' Aggregati e Filler
                            If iIndice_1 <= 10 Then
                                Try
                                    strStringTemp = dr.Item(iIndice_1 + 48).ToString
                                    fFloatTemp = System.Convert.ToSingle(strStringTemp)
                                Catch ex As System.Exception
                                    fFloatTemp = 0
                                End Try
                            End If
                            ' Legante
                            If iIndice_1 >= 11 And iIndice_1 <= 14 Then
                                Try
                                    strStringTemp = dr.Item(iIndice_1 + 49).ToString
                                    fFloatTemp = System.Convert.ToSingle(strStringTemp)
                                Catch ex As System.Exception
                                    fFloatTemp = 0
                                End Try
                            End If
                            ' Viatop
                            If iIndice_1 = 15 Then
                                Try
                                    strStringTemp = dr.Item(iIndice_1 + 44).ToString
                                    fFloatTemp = System.Convert.ToSingle(strStringTemp)
                                Catch ex As System.Exception
                                    fFloatTemp = -1.0
                                End Try
                            End If
                            ' Additivi
                            If iIndice_1 >= 16 And iIndice_1 <= 19 Then
                                fFloatTemp = 0
                            End If
                            cmd.Parameters.AddWithValue("@RIDP_Set_Toll_Prod", String.Format(fFloatTemp, "0.###").Replace(",", "."))

                            Try
                                If cmd.ExecuteNonQuery() > 0 Then
                                    'System.Windows.Forms.MessageBox.Show("Inserimento dati OK", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                Else
                                    If Not txn Is Nothing Then
                                        txn.Rollback()
                                        txn.Dispose()
                                        txn = Nothing
                                    End If
                                    bImportRes = False

                                    AddLogEvent(LOG_ERROR, "Errore nell'Inserimento Passi Ricetta.", Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                                    System.Windows.Forms.MessageBox.Show("Errore nell'Inserimento Passi Ricetta.", Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                    Exit For
                                End If
                            Catch ex As Exception
                                If Not txn Is Nothing Then
                                    txn.Rollback()
                                    txn.Dispose()
                                    txn = Nothing
                                End If
                                bImportRes = False
                                AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                                System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                Exit For
                            End Try

                            cmd.Parameters.Clear()

                        Next iIndice_1

                    Else

                        ' Intestazione report
                        ' Aggiorno l'ID Progressivo
                        iRDI_ID = iRDI_ID + 1

                        strSQL = "INSERT INTO [RepDosInt] "
                        strSQL = strSQL + "  (RDI_ID,  RDI_RIDI_ID,  RDI_Data_Ora_Start,  RDI_Silo_Destinazione,  RDI_Rapporto_F_L,  RDI_Perc_Riduz,  RDI_Net_Temp_Usc_Ess,  RDI_Net_Temp_Scar_Mix,  RDI_Net_Imp_Fuori_Toll) VALUES "
                        strSQL = strSQL + "  (@RDI_ID, @RDI_RIDI_ID, @RDI_Data_Ora_Start, @RDI_Silo_Destinazione, @RDI_Rapporto_F_L, @RDI_Perc_Riduz, @RDI_Net_Temp_Usc_Ess, @RDI_Net_Temp_Scar_Mix, @RDI_Net_Imp_Fuori_Toll)"

                        cmd.CommandText = strSQL

                        cmd.Parameters.AddWithValue("@RDI_ID", iRDI_ID)
                        cmd.Parameters.AddWithValue("@RDI_RIDI_ID", iRIDI_ID)

                        dt = Date.Parse(strDataIntest + " " + dr.Item("OraStart").ToString())
                        cmd.Parameters.AddWithValue("@RDI_Data_Ora_Start", dt)

                        cmd.Parameters.AddWithValue("@RDI_Silo_Destinazione", dr.Item("SiloDest"))

                        Try
                            strStringTemp = dr.Item("RappF_L").ToString
                            fFloatTemp = System.Convert.ToSingle(strStringTemp)
                        Catch ex As System.Exception
                            fFloatTemp = 0
                        End Try
                        cmd.Parameters.AddWithValue("@RDI_Rapporto_F_L", String.Format(fFloatTemp, "0.###").Replace(",", "."))

                        Try
                            strStringTemp = dr.Item("PercRid").ToString
                            fFloatTemp = System.Convert.ToSingle(strStringTemp)
                        Catch ex As System.Exception
                            fFloatTemp = 0
                        End Try
                        cmd.Parameters.AddWithValue("@RDI_Perc_Riduz", String.Format(fFloatTemp, "0.###").Replace(",", "."))

                        Try
                            strStringTemp = dr.Item("TUscEss").ToString
                            If Not strStringTemp.Contains("---") Then
                                fFloatTemp = System.Convert.ToSingle(strStringTemp)
                            Else
                                fFloatTemp = 0
                            End If
                        Catch ex As System.Exception
                            fFloatTemp = 0
                        End Try
                        cmd.Parameters.AddWithValue("@RDI_Net_Temp_Usc_Ess", String.Format(fFloatTemp, "0.###").Replace(",", "."))

                        Try
                            strStringTemp = dr.Item("TempScMix").ToString
                            If Not strStringTemp.Contains("---") Then
                                fFloatTemp = System.Convert.ToSingle(strStringTemp)
                            Else
                                fFloatTemp = 0
                            End If
                        Catch ex As System.Exception
                            fFloatTemp = 0
                        End Try
                        cmd.Parameters.AddWithValue("@RDI_Net_Temp_Scar_Mix", String.Format(fFloatTemp, "0.###").Replace(",", "."))

                        Try
                            strStringTemp = dr.Item("ImpFuoriT").ToString
                            fFloatTemp = System.Convert.ToSingle(strStringTemp)
                        Catch ex As System.Exception
                            fFloatTemp = 0
                        End Try
                        cmd.Parameters.AddWithValue("@RDI_Net_Imp_Fuori_Toll", String.Format(fFloatTemp, "0.###").Replace(",", "."))

                        Try
                            If cmd.ExecuteNonQuery() > 0 Then
                                'System.Windows.Forms.MessageBox.Show("Inserimento dati OK", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Else
                                If Not txn Is Nothing Then
                                    txn.Rollback()
                                    txn.Dispose()
                                    txn = Nothing
                                End If
                                bImportRes = False

                                AddLogEvent(LOG_ERROR, "Errore nell'Inserimento Intestazione Report.", Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                                System.Windows.Forms.MessageBox.Show("Errore nell'Inserimento Intestazione Report.", Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                Exit For
                            End If
                        Catch ex As Exception
                            If Not txn Is Nothing Then
                                txn.Rollback()
                                txn.Dispose()
                                txn = Nothing
                            End If
                            bImportRes = False
                            AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                            System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                            Exit For
                        End Try

                        cmd.Parameters.Clear()

                        ' Passi report
                        For iIndice_1 = 0 To 19
                            strSQL = "INSERT INTO [RepDosPassi] "
                            strSQL = strSQL + "  (RDP_RDI_ID,  RDP_NPD_Riferimento,  RDP_Net_Kg_Prod,  RDP_Net_Perc_Prod,  RDP_Net_Temp_Prod) VALUES "
                            strSQL = strSQL + "  (@RDP_RDI_ID, @RDP_NPD_Riferimento, @RDP_Net_Kg_Prod, @RDP_Net_Perc_Prod, @RDP_Net_Temp_Prod)"

                            cmd.CommandText = strSQL

                            cmd.Parameters.AddWithValue("@RDP_RDI_ID", iRDI_ID)
                            cmd.Parameters.AddWithValue("@RDP_NPD_Riferimento", dr.Table.Columns(iIndice_1 + 7).Caption.ToString)

                            Try
                                strStringTemp = dr.Item(iIndice_1 + 64).ToString
                                fFloatTemp = System.Convert.ToSingle(strStringTemp)
                            Catch ex As System.Exception
                                fFloatTemp = 0
                            End Try
                            cmd.Parameters.AddWithValue("@RDP_Net_Kg_Prod", String.Format(fFloatTemp, "0.###").Replace(",", "."))

                            If iIndice_1 <= 18 Then
                                Try
                                    strStringTemp = dr.Item(iIndice_1 + 85).ToString
                                    fFloatTemp = System.Convert.ToSingle(strStringTemp)
                                Catch ex As System.Exception
                                    fFloatTemp = -1.0
                                End Try
                            Else
                                fFloatTemp = 0
                            End If
                            cmd.Parameters.AddWithValue("@RDP_Net_Perc_Prod", String.Format(fFloatTemp, "0.###").Replace(",", "."))

                            If iIndice_1 <= 7 Then
                                Try
                                    strStringTemp = dr.Item(iIndice_1 + 104).ToString
                                    If Not strStringTemp.Contains("---") Then
                                        fFloatTemp = System.Convert.ToSingle(strStringTemp)
                                    Else
                                        fFloatTemp = 0
                                    End If
                                Catch ex As System.Exception
                                    fFloatTemp = 0
                                End Try
                            End If
                            If iIndice_1 >= 8 And iIndice_1 <= 10 Then
                                fFloatTemp = 0
                            End If
                            If iIndice_1 >= 11 And iIndice_1 <= 14 Then
                                Try
                                    strStringTemp = dr.Item(iIndice_1 + 102).ToString
                                    If Not strStringTemp.Contains("---") Then
                                        fFloatTemp = System.Convert.ToSingle(strStringTemp)
                                    Else
                                        fFloatTemp = 0
                                    End If
                                Catch ex As System.Exception
                                    fFloatTemp = 0
                                End Try
                            End If
                            If iIndice_1 >= 15 And iIndice_1 <= 19 Then
                                fFloatTemp = 0
                            End If
                            cmd.Parameters.AddWithValue("@RDP_Net_Temp_Prod", String.Format(fFloatTemp, "0.###").Replace(",", "."))

                            Try
                                If cmd.ExecuteNonQuery() > 0 Then
                                    'System.Windows.Forms.MessageBox.Show("Inserimento dati OK", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                Else
                                    If Not txn Is Nothing Then
                                        txn.Rollback()
                                        txn.Dispose()
                                        txn = Nothing
                                    End If
                                    bImportRes = False
                                    AddLogEvent(LOG_ERROR, "Errore nell'Inserimento Passi Report.", Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                                    System.Windows.Forms.MessageBox.Show("Errore nell'Inserimento Passi Report.", Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                    Exit For
                                End If
                            Catch ex As Exception
                                If Not txn Is Nothing Then
                                    txn.Rollback()
                                    txn.Dispose()
                                    txn = Nothing
                                End If
                                bImportRes = False
                                AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                                System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                Exit For
                            End Try

                            cmd.Parameters.Clear()

                        Next iIndice_1

                    End If
                    iActualInsertedRecordRecipe = iActualInsertedRecordRecipe + 1
                End If

                ' Visualizzo l'avanzamento
                iActualRecordRecipe = iActualRecordRecipe + 1
                ToolStripLabel_Avanzamento.Text = "Stato avanzamento importazione Record: " + iActualRecordRecipe.ToString + " - " + iTotalRecordRecipe.ToString + " - Job:" + iActualJobRecipe.ToString + " - Record Inseriti: " + iActualInsertedRecordRecipe.ToString
                My.Application.DoEvents()

            Next dr

            If bImportRes = True Then
                Try
                    If Not txn Is Nothing Then
                        txn.Commit()
                        txn.Dispose()
                        txn = Nothing
                    End If
                    AddLogEvent(LOG_OK, "Salvataggio Dati Di Produzione OK.", Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                    System.Windows.Forms.MessageBox.Show("Salvataggio Dati Di Produzione OK.", Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Catch ex As System.Exception
                    AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                    AddLogEvent(LOG_ERROR, "Errore nel Commit Dati Di Produzione.", Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                    System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                    System.Windows.Forms.MessageBox.Show("Errore nel Commit Dati Di Produzione.", Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                End Try
            End If
            cn.Close()

        Catch ex As Exception
            AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
            System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
        End Try

        Prod500BindingNavigatorSaveItem.Enabled = True
        RicDosNomiProdBindingNavigatorSaveItem.Enabled = True
        Prod500BindingNavigatorStopSaveItem.Enabled = False

    End Sub

    Private Sub RicDosNomiProdBindingNavigatorSaveItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RicDosNomiProdBindingNavigatorSaveItem.Click
        AddLogEvent(LOG_OK, "Esecuzione Salva Nomi Prodotti Dosaggio.", Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")

        Prod500BindingNavigatorSaveItem.Enabled = False
        RicDosNomiProdBindingNavigatorSaveItem.Enabled = False
        Prod500BindingNavigatorStopSaveItem.Enabled = True

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
            iTotalRecordRecipe = Me.DataSet_Prod500.Prod500.Rows.Count
            iActualRecordRecipe = 0
            iActualInsertedRecordRecipe = 0
            iActualJobRecipe = 0
            m_bStopSalvaInSQL = False
            ' Mi serve solo l'intestazione della tabella e non i record
            If DataSet_Prod500.Prod500.Columns.Count > 0 Then

                ' Importo solo i riferimenti del nome ed il nome lo imposto uguale al riferimento
                For iIndice_1 = 0 To 19
                    If Not DataSet_Prod500.Prod500.Columns(iIndice_1 + 7) Is DBNull.Value Then
                        If DataSet_Prod500.Prod500.Columns(iIndice_1 + 7).ToString <> "" Then
                            ' Verifico che non esista gia'
                            strSQL = "SELECT NPD_Riferimento, NPD_Nome FROM NomiProdottiDosaggio WHERE NPD_Riferimento = '" + DataSet_Prod500.Prod500.Columns(iIndice_1 + 7).Caption.ToString + "'"
                            ApriDataSet(strSQL, "", ds, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                            If ds.Tables(0).Rows.Count > 0 Then
                                ' Esiste gia', non faccio niente
                            Else
                                ' Non Esiste, la inserisco
                                strSQL = "INSERT INTO [NomiProdottiDosaggio] "
                                strSQL = strSQL + "  (NPD_Riferimento,  NPD_Nome) VALUES "
                                strSQL = strSQL + "  (@NPD_Riferimento, @NPD_Nome)"

                                cmd.CommandText = strSQL

                                cmd.Parameters.AddWithValue("@NPD_Riferimento", DataSet_Prod500.Prod500.Columns(iIndice_1 + 7).Caption.ToString)
                                cmd.Parameters.AddWithValue("@NPD_Nome", DataSet_Prod500.Prod500.Columns(iIndice_1 + 7).Caption.ToString)

                                Try
                                    If cmd.ExecuteNonQuery() > 0 Then
                                        'System.Windows.Forms.MessageBox.Show("Inserimento dati OK", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                    Else
                                        bImportRes = False

                                        AddLogEvent(LOG_ERROR, "Errore nell'Inserimento Nomi Prodotti Dosaggio.", Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                                        System.Windows.Forms.MessageBox.Show("Errore nell'Inserimento Nomi Prodotti Dosaggio.", Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
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

            End If

            If bImportRes = True Then
                AddLogEvent(LOG_OK, "Salvataggio Nomi Prodotti Dosaggio OK.", Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                System.Windows.Forms.MessageBox.Show("Salvataggio Nomi Prodotti Dosaggio OK.", Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            cn.Close()

        Catch ex As Exception
            AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
            System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
        End Try

        Prod500BindingNavigatorSaveItem.Enabled = True
        RicDosNomiProdBindingNavigatorSaveItem.Enabled = True
        Prod500BindingNavigatorStopSaveItem.Enabled = False
    End Sub

    Private Sub Prod500BindingNavigatorStopSaveItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Prod500BindingNavigatorStopSaveItem.Click
        AddLogEvent(LOG_WARNING, "Operazione Interrotta Dall'Utente.", Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
        System.Windows.Forms.MessageBox.Show("Operazione Interrotta Dall'Utente.", Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        m_bStopSalvaInSQL = True
        Prod500BindingNavigatorSaveItem.Enabled = False
        RicDosNomiProdBindingNavigatorSaveItem.Enabled = False
    End Sub

End Class