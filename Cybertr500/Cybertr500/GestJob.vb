Imports System.Data.SqlClient


Public Class GestJob

    Public Sub New()

        ' Chiamata richiesta da Progettazione Windows Form.
        InitializeComponent()

        ' Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent().
        Dim tsi As System.Windows.Forms.ToolStripItem
        Dim tss As New System.Windows.Forms.ToolStripSeparator

        ' Separatore
        ContextMenuStrip_1.Items.Add(tss)

        tsi = ContextMenuStrip_1.Items.Add("Stampa Totali")
        tsi.Name = "StampaTotaliToolStripMenuItem"

        tsi = ContextMenuStrip_1.Items.Add("Stampa Totali Senza Lista")
        tsi.Name = "StampaTotaliRidToolStripMenuItem"

        tsi = ContextMenuStrip_1.Items.Add("Stampa Completa Job")
        tsi.Name = "StampaCompletaJobToolStripMenuItem"

    End Sub

    Protected Overrides Sub ToolStripButton_Elimina_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        'If Login.ShowDialog(1, m_ldLoginData, Me.Icon, Main.APP_TITLE + " - Login", "db_ConnectionString") = Windows.Forms.DialogResult.Yes Then
        If System.Windows.Forms.MessageBox.Show("Confermi l'eliminazione dei record selezionati ?", Main.APP_TITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK Then

            AddLogEvent(LOG_OK, "Esecuzione Elimina Dati Di Produzione Selettiva.", Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")

            Main.Enabled = False
            Me.Enabled = False

            Dim strConn, strSQL As String

            Dim dgvsrc As Windows.Forms.DataGridViewSelectedRowCollection
            Dim dgvr_Tot As Windows.Forms.DataGridViewRow


            Try

                Me.Enabled = False

                ' Stato avanzamento
                dgvsrc = Me.DataGridView.SelectedRows
                Dim se As New StatoElab
                se.Show(Me)

                strConn = My.Settings.Item("db_ConnectionString")
                Dim cn As New SqlConnection(strConn)
                Dim txn As SqlTransaction
                Dim cmd As New SqlCommand
                cn.Open()
                cmd.Connection = cn

                For Each dgvr_Tot In dgvsrc
                    ' setto la transazione
                    txn = cn.BeginTransaction
                    cmd.Transaction = txn

                    ' Report passi
                    cmd.Parameters.Clear()
                    strSQL = "DELETE FROM RepDosPassi "
                    strSQL = strSQL + "FROM RepDosPassi INNER JOIN RepDosInt ON RepDosPassi.RDP_RDI_ID = RepDosInt.RDI_ID INNER JOIN RicDosInt ON RepDosInt.RDI_RIDI_ID = RicDosInt.RIDI_ID "
                    strSQL = strSQL + "WHERE RIDI_ID = @RIDI_ID"
                    cmd.CommandText = strSQL

                    cmd.Parameters.AddWithValue("@RIDI_ID", dgvr_Tot.Cells("RIDI_ID").Value)

                    cmd.ExecuteNonQuery()
                    se.NewStep()
                    My.Application.DoEvents()

                    ' Report intestazioni
                    cmd.Parameters.Clear()
                    strSQL = "DELETE FROM RepDosInt "
                    strSQL = strSQL + "FROM RepDosInt INNER JOIN RicDosInt ON RepDosInt.RDI_RIDI_ID = RicDosInt.RIDI_ID "
                    strSQL = strSQL + "WHERE RIDI_ID = @RIDI_ID"
                    cmd.CommandText = strSQL

                    cmd.Parameters.AddWithValue("@RIDI_ID", dgvr_Tot.Cells("RIDI_ID").Value)

                    cmd.ExecuteNonQuery()
                    se.NewStep()
                    My.Application.DoEvents()

                    ' Ricetta passi
                    cmd.Parameters.Clear()
                    strSQL = "DELETE FROM RicDosPassi "
                    strSQL = strSQL + "FROM RicDosPassi INNER JOIN RicDosInt ON RicDosPassi.RIDP_RIDI_ID = RicDosInt.RIDI_ID "
                    strSQL = strSQL + "WHERE RIDI_ID = @RIDI_ID"
                    cmd.CommandText = strSQL

                    cmd.Parameters.AddWithValue("@RIDI_ID", dgvr_Tot.Cells("RIDI_ID").Value)

                    cmd.ExecuteNonQuery()
                    se.NewStep()
                    My.Application.DoEvents()

                    ' Ricetta intestazioni
                    cmd.Parameters.Clear()
                    strSQL = "DELETE FROM RicDosInt "
                    strSQL = strSQL + "WHERE RIDI_ID = @RIDI_ID"
                    cmd.CommandText = strSQL

                    cmd.Parameters.AddWithValue("@RIDI_ID", dgvr_Tot.Cells("RIDI_ID").Value)

                    cmd.ExecuteNonQuery()
                    se.NewStep()
                    My.Application.DoEvents()

                    txn.Commit()

                    If se.Annulla = True Then
                        Exit For
                    End If

                Next dgvr_Tot

                se.Close()

                Me.Enabled = True

                CostruisciSQL()
                EseguiSQL()
                PopolaGriglia()

                AddLogEvent(LOG_OK, "Eliminazione Dati Di Produzione Selettiva OK.", Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                System.Windows.Forms.MessageBox.Show("Eliminazione Dati Di Produzione Selettiva OK.", Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception

                AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End Try

            Me.Enabled = True
            Main.Enabled = True

        End If

    End Sub

    Protected Overrides Sub ToolStripButton_Modifica_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Me.DesignMode = False Then
        End If
    End Sub

    Protected Overrides Sub ContextMenuStrip_ItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs)
        If Me.DesignMode = False Then
            Main.Enabled = False
            Me.Enabled = False
            If e.ClickedItem.Name = "StampaTotaliToolStripMenuItem" Then
                StampaCompletaJob(1)
            ElseIf e.ClickedItem.Name = "StampaTotaliRidToolStripMenuItem" Then
                StampaCompletaJob(2)
            ElseIf e.ClickedItem.Name = "StampaCompletaJobToolStripMenuItem" Then
                StampaCompletaJob(0)
            End If
            Me.Enabled = True
            Main.Enabled = True
        End If
        MyBase.ContextMenuStrip_ItemClicked(sender, e)
    End Sub

    Private Sub StampaCompletaJob(ByVal iSoloTotali As Integer)
        Dim iIndice_1 As Integer
        Dim iIndice_2 As Integer
        Dim strHTML As String
        Dim strNomeFile As String

        Dim strSQL, strConn As String
        Dim ds_RicPre_Int As New DataSet
        Dim ds_RicPre_Passi As New DataSet
        Dim dr_RicPre_Int As DataRow
        Dim dr_RicPre_Passi As DataRow
        Dim ds_RicPre_Tot As New DataSet
        Dim dr_RicPre_Tot As DataRow
        Dim dr_RicPre_Tot_Select() As DataRow
        Dim b_ds_RicPre_Tot_Row_Add As Boolean
        Dim obj_Value(1) As Object
        ds_RicPre_Tot.Tables.Add()
        ds_RicPre_Tot.Tables(0).Columns.Add("Nome Prodotto")
        ds_RicPre_Tot.Tables(0).Columns.Add("Consumo Umido")

        Dim ds_Ric_Int As New DataSet
        Dim ds_Rep_Int As New DataSet
        Dim ds_Ric_Rep_Passi As New DataSet
        Dim dr_Ric_Int As DataRow
        Dim dr_Rep_Int As DataRow

        Dim dr_Ric_Rep_Passi As DataRow

        Dim ds_Tot As New DataSet
        Dim dr_Tot As DataRow
        Dim bFirstOR As Boolean

        Dim dc As DataColumn

        Dim dgvr_Tot As Windows.Forms.DataGridViewRow

        Dim iRPDI_ID As Integer

        Dim fConsumiParziali As Single
        Dim fConsumiTotali As Single
        Dim fConsumiParzialiUmidita As Single
        Dim fConsumiParzialiUmiditaPerc As Single
        Dim fConsumiTotaliUmidita As Single
        Dim fPercDos As Single

        Dim dgvsrc As Windows.Forms.DataGridViewSelectedRowCollection

        Dim hFile As System.IO.StreamWriter

        Dim dtMIN As DateTime
        Dim dtMAX As DateTime

        ' Stato avanzamento
        dgvsrc = Me.DataGridView.SelectedRows
        Dim se As New StatoElab
        se.Show(Me)

        strHTML = ""
        'strNomeFile = My.Application.Info.DirectoryPath.ToString() + "\" + Main.APP_TITLE + " - Rapporto Di Dosaggio" + ".html"
        strNomeFile = Environment.GetEnvironmentVariables().Item("TEMP").ToString() + "\" + Main.APP_TITLE + " - Rapporto Di Dosaggio" + ".html"
        Try
            hFile = My.Computer.FileSystem.OpenTextFileWriter(strNomeFile, False)
            hFile.WriteLine(strHTML)
            'My.Computer.FileSystem.WriteAllText(strNomeFile, strHTML, False)
            strHTML = ""

        Catch ex As Exception
            AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
            System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            se.Close()
            Exit Sub
        End Try

        'verificare questa funzioni con i dati attualmente caricati in tabella
        strHTML = "<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0//EN""" + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + """http://www.w3.org/TR/REC-html140/strict.dtd"">" + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + "<HTML>" + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + "<HEAD>" + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + "<TITLE>" + Main.APP_TITLE + " - Rapporto Di Dosaggio" + "</TITLE>" + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + "</HEAD>" + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + "<BODY>" + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + "<H1>" + Main.APP_TITLE + " - Rapporto Di Dosaggio" + "</H1>" + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + "<H1>" + "</H1>" + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + "<H1>" + "Elaborato il: " + Date.Now.ToString + "</H1>" + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + "<H1>" + "</H1>" + Microsoft.VisualBasic.vbCrLf

        ' Verifico che ci sia qualcosa di selezionato
        If dgvsrc.Count > 0 Then
            If iSoloTotali = 0 Then
            ElseIf iSoloTotali = 1 Then
                strHTML = strHTML + "<TABLE BORDER=1 WIDTH=100%>" + Microsoft.VisualBasic.vbCrLf
                strHTML = strHTML + "<TR>" + Microsoft.VisualBasic.vbCrLf
                For iIndice_1 = 0 To Me.DataGridView.ColumnCount - 1
                    If Me.DataGridView.Columns(iIndice_1).Visible = True Then
                        strHTML = strHTML + "<TD>"
                        strHTML = strHTML + Me.DataGridView.Columns(iIndice_1).HeaderText.ToString
                        strHTML = strHTML + "</TD>" + Microsoft.VisualBasic.vbCrLf
                    End If
                    se.NewStep()
                    My.Application.DoEvents()
                Next iIndice_1
                strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf
                If dgvsrc.Count > 0 Then
                    For iIndice_1 = 0 To dgvsrc.Count - 1
                        strHTML = strHTML + "<TR>" + Microsoft.VisualBasic.vbCrLf
                        For iIndice_2 = 0 To Me.DataGridView.ColumnCount - 1
                            If Me.DataGridView.Columns(iIndice_2).Visible = True Then
                                strHTML = strHTML + "<TD>"
                                strHTML = strHTML + dgvsrc.Item((dgvsrc.Count - 1) - iIndice_1).Cells(iIndice_2).Value.ToString
                                strHTML = strHTML + "</TD>" + Microsoft.VisualBasic.vbCrLf
                            End If
                        Next iIndice_2
                        strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf

                        Try
                            hFile.WriteLine(strHTML)
                            strHTML = ""

                        Catch ex As Exception
                            AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, m_ldLoginData, m_strFormTitle, m_strConnStringParamName)
                            System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, m_strFormTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                            se.Close()
                            Exit Sub
                        End Try
                        se.NewStep()
                        My.Application.DoEvents()
                    Next iIndice_1
                End If
                strHTML = strHTML + "</TABLE>" + Microsoft.VisualBasic.vbCrLf

                Try
                    hFile.WriteLine(strHTML)
                    strHTML = ""

                Catch ex As Exception
                    AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, m_ldLoginData, m_strFormTitle, m_strConnStringParamName)
                    System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, m_strFormTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                    se.Close()
                    Exit Sub
                End Try
            ElseIf iSoloTotali = 2 Then
                For Each dgvr_Tot In dgvsrc
                    dtMIN = dgvr_Tot.Cells("RIDI_Data_Ora_Start").Value
                    dtMAX = dgvr_Tot.Cells("RIDI_Data_Ora_Start").Value
                    Exit For
                Next dgvr_Tot
                For Each dgvr_Tot In dgvsrc
                    If dgvr_Tot.Cells("RIDI_Data_Ora_Start").Value < dtMIN Then
                        dtMIN = dgvr_Tot.Cells("RIDI_Data_Ora_Start").Value
                    End If
                    If dgvr_Tot.Cells("RIDI_Data_Ora_Start").Value > dtMAX Then
                        dtMAX = dgvr_Tot.Cells("RIDI_Data_Ora_Start").Value
                    End If
                Next dgvr_Tot

                strHTML = strHTML + "<TR>" + Microsoft.VisualBasic.vbCrLf
                strHTML = strHTML + "<DIV align=center>" + Microsoft.VisualBasic.vbCrLf
                strHTML = strHTML + "<H1>" + "---------------------------" + "</H1>" + Microsoft.VisualBasic.vbCrLf
                strHTML = strHTML + "<H1>" + "Dal: " + dtMIN.ToString() + " - Al: " + dtMAX.ToString() + "</H1>" + Microsoft.VisualBasic.vbCrLf
                strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf
            End If

            For Each dgvr_Tot In dgvsrc

                If iSoloTotali = 0 Then
                    strHTML = strHTML + "<DIV align=center>" + Microsoft.VisualBasic.vbCrLf
                    strHTML = strHTML + "<H1>" + "</H1>" + Microsoft.VisualBasic.vbCrLf
                    strHTML = strHTML + "<H1>" + "</H1>" + Microsoft.VisualBasic.vbCrLf
                    strHTML = strHTML + "<H1>" + "---------------------------" + "</H1>" + Microsoft.VisualBasic.vbCrLf
                    strHTML = strHTML + "<H1>" + "</H1>" + Microsoft.VisualBasic.vbCrLf
                    strHTML = strHTML + "<TR" + Microsoft.VisualBasic.vbCrLf
                    strHTML = strHTML + "<TD>" + Microsoft.VisualBasic.vbCrLf
                    strHTML = strHTML + "<H1>" + "Job: " + dgvr_Tot.Cells("RIDI_Job_Descrizione").Value.ToString() + "</H1>" + Microsoft.VisualBasic.vbCrLf
                    strHTML = strHTML + "</TD>" + Microsoft.VisualBasic.vbCrLf
                    strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf
                End If

                Try
                    hFile.WriteLine(strHTML)
                    strHTML = ""

                Catch ex As Exception
                    AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                    System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                    se.Close()
                    Exit Sub
                End Try

                ' ***************** Predosaggio *****************
                ' Prelevo l'ID da stampare
                iRPDI_ID = GENERICA_DESCRIZIONE_NUMERICA("RPDI_ID", "RicPreDosInt", "RPDI_Nome", dgvr_Tot.Cells("RIDI_Nome_Ricetta_Predosaggio").Value.ToString, m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                strSQL = "SELECT RPDI_ID, RPDI_Nome, RPDI_Descrizione, RPDI_PredRicInV, RPDI_PrRicSeEl, RPDI_EsclBrImp, RPDI_TmpRicPrd, RPDI_MRSuAnEdEl "
                strSQL = strSQL + "FROM RicPreDosInt "
                strSQL = strSQL + "WHERE RPDI_ID = " + iRPDI_ID.ToString
                ApriDataSet(strSQL, "", ds_RicPre_Int, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")

                ' Verifico che effettivamente esista una ricetta di predosaggio
                If ds_RicPre_Int.Tables(0).Rows.Count > 0 Then
                    If iSoloTotali = 0 Then
                        ' Intestazione Ricetta
                        strHTML = strHTML + "<DIV align=left>" + Microsoft.VisualBasic.vbCrLf

                        strHTML = strHTML + "<TR" + Microsoft.VisualBasic.vbCrLf
                        strHTML = strHTML + "<TD>" + Microsoft.VisualBasic.vbCrLf
                        strHTML = strHTML + "<H1>" + "Predosaggio" + "</H1>" + Microsoft.VisualBasic.vbCrLf
                        strHTML = strHTML + "</TD>" + Microsoft.VisualBasic.vbCrLf
                        strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf

                        strHTML = strHTML + "<TABLE BORDER=1 WIDTH=100%>" + Microsoft.VisualBasic.vbCrLf
                        strHTML = strHTML + "<TR>" + Microsoft.VisualBasic.vbCrLf
                        For Each dc In ds_RicPre_Int.Tables(0).Columns
                            ' Inserisco le caption
                            strConn = My.Settings.Item("db_ConnectionString").ToString
                            strSQL = "SELECT DCC_Caption FROM [DatiCaptionColonne] WHERE DCC_Nome_Colonna ='" + dc.ColumnName.ToString + "'"
                            Dim cn As New SqlConnection(strConn)
                            cn.Open()
                            Dim cmd As New SqlCommand(strSQL, cn)
                            Dim rdr As SqlDataReader = cmd.ExecuteReader()
                            If rdr.HasRows = True Then
                                rdr.Read()
                                dc.Caption = rdr.Item(0).ToString
                            End If
                            rdr.Close()
                            cn.Close()

                            If dc.ColumnName.ToString() <> dc.Caption.ToString() Then
                                strHTML = strHTML + "<TD>"
                                strHTML = strHTML + dc.Caption.ToString
                                strHTML = strHTML + "</TD>" + Microsoft.VisualBasic.vbCrLf
                            End If
                        Next dc
                        strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf
                        Try
                            hFile.WriteLine(strHTML)
                            strHTML = ""

                        Catch ex As Exception
                            AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                            System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                            se.Close()
                            Exit Sub
                        End Try

                        For Each dr_RicPre_Int In ds_RicPre_Int.Tables(0).Rows
                            strHTML = strHTML + "<TR>" + Microsoft.VisualBasic.vbCrLf
                            For iIndice_1 = 0 To ds_RicPre_Int.Tables(0).Columns.Count - 1
                                If dr_RicPre_Int.Table.Columns(iIndice_1).ColumnName.ToString() <> dr_RicPre_Int.Table.Columns(iIndice_1).Caption.ToString() Then
                                    strHTML = strHTML + "<TD>"
                                    strHTML = strHTML + dr_RicPre_Int.Item(iIndice_1).ToString
                                    strHTML = strHTML + "</TD>" + Microsoft.VisualBasic.vbCrLf
                                End If
                            Next iIndice_1
                            strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf
                        Next dr_RicPre_Int
                        strHTML = strHTML + "</TABLE>" + Microsoft.VisualBasic.vbCrLf
                        Try
                            hFile.WriteLine(strHTML)
                            strHTML = ""

                        Catch ex As Exception
                            AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                            System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                            se.Close()
                            Exit Sub
                        End Try
                    End If

                    ' Passi Ricetta
                    strSQL = "SELECT RPDP_ID, RPDP_Nome_Prodotto, RPDP_Set_Perc, RPDP_Set_Toll, RPDP_Set_Rit_Start, RPDP_Set_Rit_Stop, RPDP_Set_Ponderale "
                    strSQL = strSQL + "FROM RicPreDosPassi "
                    strSQL = strSQL + "WHERE RPDP_RPDI_ID = " + ds_RicPre_Int.Tables(0).Rows(0).Item("RPDI_ID").ToString
                    ApriDataSet(strSQL, "", ds_RicPre_Passi, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")

                    If iSoloTotali = 0 Then
                        strHTML = strHTML + "<TABLE BORDER=1 WIDTH=100%>" + Microsoft.VisualBasic.vbCrLf
                        strHTML = strHTML + "<TR>" + Microsoft.VisualBasic.vbCrLf
                        For Each dc In ds_RicPre_Passi.Tables(0).Columns
                            ' Inserisco le caption
                            strConn = My.Settings.Item("db_ConnectionString").ToString
                            strSQL = "SELECT DCC_Caption FROM [DatiCaptionColonne] WHERE DCC_Nome_Colonna ='" + dc.ColumnName.ToString + "'"
                            Dim cn As New SqlConnection(strConn)
                            cn.Open()
                            Dim cmd As New SqlCommand(strSQL, cn)
                            Dim rdr As SqlDataReader = cmd.ExecuteReader()
                            If rdr.HasRows = True Then
                                rdr.Read()
                                dc.Caption = rdr.Item(0).ToString
                            End If
                            rdr.Close()
                            cn.Close()

                            If dc.ColumnName.ToString() <> dc.Caption.ToString() Then
                                strHTML = strHTML + "<TD>"
                                strHTML = strHTML + dc.Caption.ToString
                                strHTML = strHTML + "</TD>" + Microsoft.VisualBasic.vbCrLf
                            End If
                        Next dc
                        strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf
                        Try
                            hFile.WriteLine(strHTML)
                            strHTML = ""

                        Catch ex As Exception
                            AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                            System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                            se.Close()
                            Exit Sub
                        End Try

                        For Each dr_RicPre_Passi In ds_RicPre_Passi.Tables(0).Rows
                            strHTML = strHTML + "<TR>" + Microsoft.VisualBasic.vbCrLf
                            For iIndice_1 = 0 To ds_RicPre_Passi.Tables(0).Columns.Count - 1
                                If dr_RicPre_Passi.Table.Columns(iIndice_1).ColumnName.ToString() <> dr_RicPre_Passi.Table.Columns(iIndice_1).Caption.ToString() Then
                                    strHTML = strHTML + "<TD>"
                                    strHTML = strHTML + dr_RicPre_Passi.Item(iIndice_1).ToString
                                    strHTML = strHTML + "</TD>" + Microsoft.VisualBasic.vbCrLf
                                End If
                            Next iIndice_1
                            strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf

                        Next dr_RicPre_Passi
                        strHTML = strHTML + "</TABLE>" + Microsoft.VisualBasic.vbCrLf
                        Try
                            hFile.WriteLine(strHTML)
                            strHTML = ""

                        Catch ex As Exception
                            AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                            System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                            se.Close()
                            Exit Sub
                        End Try

                        strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf
                    End If
                Else
                    If iSoloTotali = 0 Then
                        ' Intestazione Ricetta
                        strHTML = strHTML + "<DIV align=left>" + Microsoft.VisualBasic.vbCrLf
                        strHTML = strHTML + "<TR" + Microsoft.VisualBasic.vbCrLf
                        strHTML = strHTML + "<TD>" + Microsoft.VisualBasic.vbCrLf
                        strHTML = strHTML + "<H1>" + "La Ricetta Di Predosaggio: " + DataGridView.CurrentRow.Cells("RIDI_Nome_Ricetta_Predosaggio").Value.ToString + " non esiste" + "</H1>" + Microsoft.VisualBasic.vbCrLf
                        strHTML = strHTML + "</TD>" + Microsoft.VisualBasic.vbCrLf
                        strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf
                        Try
                            hFile.WriteLine(strHTML)
                            strHTML = ""

                        Catch ex As Exception
                            AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                            System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                            se.Close()
                            Exit Sub
                        End Try
                    End If
                End If
                se.NewStep()
                My.Application.DoEvents()
                If iSoloTotali = 0 Then
                    ' ***************** Dosaggio *****************

                    strHTML = strHTML + "<TR" + Microsoft.VisualBasic.vbCrLf
                    strHTML = strHTML + "<TD>" + Microsoft.VisualBasic.vbCrLf
                    strHTML = strHTML + "<H1>" + "Dosaggio" + "</H1>" + Microsoft.VisualBasic.vbCrLf
                    strHTML = strHTML + "</TD>" + Microsoft.VisualBasic.vbCrLf
                    strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf
                End If

                ' Prelevo l'ID da stampare
                ' Popolo il dataset dell'intestazione della ricetta
                strSQL = "SELECT RIDI_ID, RIDI_Nome_Operatore, RIDI_Nome_Ricetta, RIDI_Nome_Ricetta_Predosaggio, RIDI_Job_Descrizione, RIDI_Data_Ora_Start, RIDI_Silo_Destinazione, RIDI_Rapporto_F_L "
                strSQL = strSQL + "FROM RicDosInt "
                strSQL = strSQL + "WHERE RIDI_ID = " + dgvr_Tot.Cells("RIDI_ID").Value.ToString
                ApriDataSet(strSQL, "", ds_Ric_Int, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")

                If ds_Ric_Int.Tables.Count > 0 Then

                    ' Popolo il dataset dell'intestazione del report
                    strSQL = "SELECT RDI_ID, RDI_Data_Ora_Start, RDI_Silo_Destinazione, RDI_Rapporto_F_L, RDI_Perc_Riduz, RDI_Net_Temp_Usc_Ess, RDI_Net_Temp_Scar_Mix, RDI_Net_Imp_Fuori_Toll "
                    strSQL = strSQL + "FROM RepDosInt "
                    strSQL = strSQL + "WHERE RDI_RIDI_ID = " + ds_Ric_Int.Tables(0).Rows(0).Item("RIDI_ID").ToString
                    ApriDataSet(strSQL, "", ds_Rep_Int, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")


                    If ds_Rep_Int.Tables.Count > 0 Then

                        For Each dr_Rep_Int In ds_Rep_Int.Tables(0).Rows

                            If iSoloTotali = 0 Then

                                ' Intestazione Ricetta
                                strHTML = strHTML + "<TR" + Microsoft.VisualBasic.vbCrLf
                                strHTML = strHTML + "<TD>" + Microsoft.VisualBasic.vbCrLf
                                strHTML = strHTML + "."
                                strHTML = strHTML + "</TD>" + Microsoft.VisualBasic.vbCrLf
                                strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf

                                strHTML = strHTML + "<TABLE BORDER=1 WIDTH=100%>" + Microsoft.VisualBasic.vbCrLf
                                strHTML = strHTML + "<TR>" + Microsoft.VisualBasic.vbCrLf
                                For Each dc In ds_Ric_Int.Tables(0).Columns
                                    ' Inserisco le caption
                                    strConn = My.Settings.Item("db_ConnectionString").ToString
                                    strSQL = "SELECT DCC_Caption FROM [DatiCaptionColonne] WHERE DCC_Nome_Colonna ='" + dc.ColumnName.ToString + "'"
                                    Dim cn As New SqlConnection(strConn)
                                    cn.Open()
                                    Dim cmd As New SqlCommand(strSQL, cn)
                                    Dim rdr As SqlDataReader = cmd.ExecuteReader()
                                    If rdr.HasRows = True Then
                                        rdr.Read()
                                        dc.Caption = rdr.Item(0).ToString
                                    End If
                                    rdr.Close()
                                    cn.Close()

                                    If dc.ColumnName.ToString() <> dc.Caption.ToString() Then
                                        strHTML = strHTML + "<TD>"
                                        strHTML = strHTML + dc.Caption.ToString
                                        strHTML = strHTML + "</TD>" + Microsoft.VisualBasic.vbCrLf
                                    End If
                                Next dc
                                strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf
                                Try
                                    hFile.WriteLine(strHTML)
                                    strHTML = ""

                                Catch ex As Exception
                                    AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                                    System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                    se.Close()
                                    Exit Sub
                                End Try

                                For Each dr_Ric_Int In ds_Ric_Int.Tables(0).Rows
                                    strHTML = strHTML + "<TR>" + Microsoft.VisualBasic.vbCrLf
                                    For iIndice_1 = 0 To ds_Ric_Int.Tables(0).Columns.Count - 1
                                        If dr_Ric_Int.Table.Columns(iIndice_1).ColumnName.ToString() <> dr_Ric_Int.Table.Columns(iIndice_1).Caption.ToString() Then
                                            strHTML = strHTML + "<TD>"
                                            strHTML = strHTML + dr_Ric_Int.Item(iIndice_1).ToString
                                            strHTML = strHTML + "</TD>" + Microsoft.VisualBasic.vbCrLf
                                        End If
                                    Next iIndice_1
                                    strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf

                                Next dr_Ric_Int
                                strHTML = strHTML + "</TABLE>" + Microsoft.VisualBasic.vbCrLf
                                Try
                                    hFile.WriteLine(strHTML)
                                    strHTML = ""

                                Catch ex As Exception
                                    AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                                    System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                    se.Close()
                                    Exit Sub
                                End Try

                                se.NewStep()
                                My.Application.DoEvents()

                                ' Intestazione Report
                                strHTML = strHTML + "<TABLE BORDER=1 WIDTH=100%>" + Microsoft.VisualBasic.vbCrLf
                                strHTML = strHTML + "<TR>" + Microsoft.VisualBasic.vbCrLf
                                For Each dc In ds_Rep_Int.Tables(0).Columns
                                    ' Inserisco le caption
                                    strConn = My.Settings.Item("db_ConnectionString").ToString
                                    strSQL = "SELECT DCC_Caption FROM [DatiCaptionColonne] WHERE DCC_Nome_Colonna ='" + dc.ColumnName.ToString + "'"
                                    Dim cn As New SqlConnection(strConn)
                                    cn.Open()
                                    Dim cmd As New SqlCommand(strSQL, cn)
                                    Dim rdr As SqlDataReader = cmd.ExecuteReader()
                                    If rdr.HasRows = True Then
                                        rdr.Read()
                                        dc.Caption = rdr.Item(0).ToString
                                    End If
                                    rdr.Close()
                                    cn.Close()

                                    If dc.ColumnName.ToString() <> dc.Caption.ToString() Then
                                        strHTML = strHTML + "<TD>"
                                        strHTML = strHTML + dc.Caption.ToString
                                        strHTML = strHTML + "</TD>" + Microsoft.VisualBasic.vbCrLf
                                    End If
                                Next dc
                                strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf
                                Try
                                    hFile.WriteLine(strHTML)
                                    strHTML = ""

                                Catch ex As Exception
                                    AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                                    System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                    se.Close()
                                    Exit Sub
                                End Try

                                strHTML = strHTML + "<TR>" + Microsoft.VisualBasic.vbCrLf
                                For iIndice_1 = 0 To ds_Rep_Int.Tables(0).Columns.Count - 1
                                    If dr_Rep_Int.Table.Columns(iIndice_1).ColumnName.ToString() <> dr_Rep_Int.Table.Columns(iIndice_1).Caption.ToString() Then
                                        strHTML = strHTML + "<TD>"
                                        If dr_Rep_Int.Table.Columns(iIndice_1).ColumnName.ToString() = "RDI_Data_Ora_Start" Then
                                            'strHTML = strHTML + "<b>"
                                            strHTML = strHTML + "<H1>" + dr_Rep_Int.Item(iIndice_1).ToString + "</H1>"
                                            'strHTML = strHTML + "<normal>"
                                        Else
                                            strHTML = strHTML + dr_Rep_Int.Item(iIndice_1).ToString
                                        End If
                                        strHTML = strHTML + "</TD>" + Microsoft.VisualBasic.vbCrLf
                                    End If
                                Next iIndice_1
                                strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf
                                strHTML = strHTML + "</TABLE>" + Microsoft.VisualBasic.vbCrLf
                                Try
                                    hFile.WriteLine(strHTML)
                                    strHTML = ""

                                Catch ex As Exception
                                    AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                                    System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                    se.Close()
                                    Exit Sub
                                End Try
                            End If

                            se.NewStep()
                            My.Application.DoEvents()

                            ' Passi Ricetta Report
                            strSQL = "SELECT RepDosPassi.RDP_ID, RepDosPassi.RDP_RDI_ID, RicDosPassi.RIDP_NPD_Riferimento, RicDosPassi.RIDP_Set_Kg_Prod, RepDosPassi.RDP_Net_Kg_Prod, RicDosPassi.RIDP_Set_Perc_Prod, RepDosPassi.RDP_Net_Perc_Prod, ROUND((RicDosPassi.RIDP_Set_Perc_Prod - RepDosPassi.RDP_Net_Perc_Prod),1)AS DiffPerc, RicDosPassi.RIDP_Set_Toll_Prod, RepDosPassi.RDP_Net_Temp_Prod "
                            strSQL = strSQL + "FROM RicDosInt INNER JOIN RepDosInt ON RicDosInt.RIDI_ID = RepDosInt.RDI_RIDI_ID INNER JOIN RicDosPassi ON RicDosInt.RIDI_ID = RicDosPassi.RIDP_RIDI_ID INNER JOIN RepDosPassi ON RepDosInt.RDI_ID = RepDosPassi.RDP_RDI_ID AND RicDosPassi.RIDP_NPD_Riferimento = RepDosPassi.RDP_NPD_Riferimento "
                            strSQL = strSQL + "WHERE RepDosPassi.RDP_RDI_ID = " + ds_Rep_Int.Tables(0).Rows(0).Item("RDI_ID").ToString
                            ApriDataSet(strSQL, "", ds_Ric_Rep_Passi, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")

                            If iSoloTotali = 0 Then

                                strHTML = strHTML + "<TABLE BORDER=1 WIDTH=100%>" + Microsoft.VisualBasic.vbCrLf
                                strHTML = strHTML + "<TR>" + Microsoft.VisualBasic.vbCrLf
                                For Each dc In ds_Ric_Rep_Passi.Tables(0).Columns
                                    ' Inserisco le caption
                                    strConn = My.Settings.Item("db_ConnectionString").ToString
                                    strSQL = "SELECT DCC_Caption FROM [DatiCaptionColonne] WHERE DCC_Nome_Colonna ='" + dc.ColumnName.ToString + "'"
                                    Dim cn As New SqlConnection(strConn)
                                    cn.Open()
                                    Dim cmd As New SqlCommand(strSQL, cn)
                                    Dim rdr As SqlDataReader = cmd.ExecuteReader()
                                    If rdr.HasRows = True Then
                                        rdr.Read()
                                        dc.Caption = rdr.Item(0).ToString
                                    End If
                                    rdr.Close()
                                    cn.Close()

                                    If dc.ColumnName.ToString() <> dc.Caption.ToString() Then
                                        strHTML = strHTML + "<TD>"
                                        strHTML = strHTML + dc.Caption.ToString
                                        strHTML = strHTML + "</TD>" + Microsoft.VisualBasic.vbCrLf
                                    End If
                                Next dc
                                strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf
                                Try
                                    hFile.WriteLine(strHTML)
                                    strHTML = ""

                                Catch ex As Exception
                                    AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                                    System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                    se.Close()
                                    Exit Sub
                                End Try

                                For Each dr_Ric_Rep_Passi In ds_Ric_Rep_Passi.Tables(0).Rows
                                    strHTML = strHTML + "<TR>" + Microsoft.VisualBasic.vbCrLf
                                    For iIndice_1 = 0 To ds_Ric_Rep_Passi.Tables(0).Columns.Count - 1
                                        If dr_Ric_Rep_Passi.Table.Columns(iIndice_1).ColumnName.ToString() <> dr_Ric_Rep_Passi.Table.Columns(iIndice_1).Caption.ToString() Then
                                            strHTML = strHTML + "<TD>"
                                            ' Prelevo i nomi dei prodotti
                                            If iIndice_1 = 2 Then
                                                strHTML = strHTML + GENERICA_DESCRIZIONE_STRINGA("NPD_Nome", "NomiProdottiDosaggio", "NPD_Riferimento", dr_Ric_Rep_Passi.Item(iIndice_1).ToString, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                                            Else
                                                strHTML = strHTML + dr_Ric_Rep_Passi.Item(iIndice_1).ToString()
                                            End If
                                            strHTML = strHTML + "</TD>" + Microsoft.VisualBasic.vbCrLf
                                        End If
                                    Next iIndice_1
                                    strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf

                                Next dr_Ric_Rep_Passi
                                strHTML = strHTML + "</TABLE>" + Microsoft.VisualBasic.vbCrLf
                                Try
                                    hFile.WriteLine(strHTML)
                                    strHTML = ""

                                Catch ex As Exception
                                    AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                                    System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                    se.Close()
                                    Exit Sub
                                End Try
                            End If

                            se.NewStep()
                            My.Application.DoEvents()

                        Next dr_Rep_Int
                    End If

                    ' Totali
                    fConsumiTotali = 0

                    strSQL = "SELECT NPD_ID, SUM(RDP_Net_Kg_Prod) as ""Totale_Net_Kg_Prod"" "
                    strSQL = strSQL + "FROM RepDosPassi INNER JOIN RepDosInt ON RepDosPassi.RDP_RDI_ID = RepDosInt.RDI_ID INNER JOIN NomiProdottiDosaggio ON RepDosPassi.RDP_NPD_Riferimento = NomiProdottiDosaggio.NPD_Riferimento "
                    strSQL = strSQL + "WHERE RDI_RIDI_ID = " + dgvr_Tot.Cells("RIDI_ID").Value.ToString + " "
                    strSQL = strSQL + "GROUP BY(NPD_ID) "
                    strSQL = strSQL + "ORDER BY(NPD_ID) "
                    ApriDataSet(strSQL, "", ds_Tot, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                    If iSoloTotali = 0 Then
                        If ds_Tot.Tables.Count > 0 Then

                            strHTML = strHTML + "<TR" + Microsoft.VisualBasic.vbCrLf
                            strHTML = strHTML + "<TD>" + Microsoft.VisualBasic.vbCrLf
                            strHTML = strHTML + "."
                            strHTML = strHTML + "</TD>" + Microsoft.VisualBasic.vbCrLf
                            strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf

                            strHTML = strHTML + "<TR" + Microsoft.VisualBasic.vbCrLf
                            strHTML = strHTML + "<TD>" + Microsoft.VisualBasic.vbCrLf
                            strHTML = strHTML + "<H1>" + "Totale Consumi Job: " + dgvr_Tot.Cells("RIDI_Job_Descrizione").Value.ToString() + "</H1>" + Microsoft.VisualBasic.vbCrLf
                            strHTML = strHTML + "</TD>" + Microsoft.VisualBasic.vbCrLf
                            strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf

                            strHTML = strHTML + "<TABLE BORDER=1 WIDTH=100%>" + Microsoft.VisualBasic.vbCrLf
                            strHTML = strHTML + "<TR>" + Microsoft.VisualBasic.vbCrLf
                            For Each dc In ds_Tot.Tables(0).Columns
                                ' Inserisco le caption
                                strConn = My.Settings.Item("db_ConnectionString").ToString
                                strSQL = "SELECT DCC_Caption FROM [DatiCaptionColonne] WHERE DCC_Nome_Colonna ='" + dc.ColumnName.ToString + "'"
                                Dim cn As New SqlConnection(strConn)
                                cn.Open()
                                Dim cmd As New SqlCommand(strSQL, cn)
                                Dim rdr As SqlDataReader = cmd.ExecuteReader()
                                If rdr.HasRows = True Then
                                    rdr.Read()
                                    dc.Caption = rdr.Item(0).ToString
                                End If
                                rdr.Close()
                                cn.Close()

                                strHTML = strHTML + "<TD>"
                                strHTML = strHTML + dc.Caption.ToString
                                strHTML = strHTML + "</TD>" + Microsoft.VisualBasic.vbCrLf
                            Next dc
                            strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf
                            Try
                                hFile.WriteLine(strHTML)
                                strHTML = ""

                            Catch ex As Exception
                                AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                                System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                se.Close()
                                Exit Sub
                            End Try

                            For Each dr_Tot In ds_Tot.Tables(0).Rows
                                strHTML = strHTML + "<TR>" + Microsoft.VisualBasic.vbCrLf
                                For iIndice_1 = 0 To ds_Tot.Tables(0).Columns.Count - 1
                                    strHTML = strHTML + "<TD>"
                                    ' Prelevo i nomi dei prodotti
                                    If iIndice_1 = 0 Then
                                        strHTML = strHTML + GENERICA_DESCRIZIONE_STRINGA("NPD_Nome", "NomiProdottiDosaggio", "NPD_ID", dr_Tot.Item(iIndice_1).ToString, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                                    Else
                                        fConsumiTotali = fConsumiTotali + dr_Tot.Item(iIndice_1)
                                        fConsumiParziali = dr_Tot.Item(iIndice_1)
                                        strHTML = strHTML + (fConsumiParziali / 1000.0).ToString("#,#0.0") + " T"

                                    End If
                                    strHTML = strHTML + "</TD>" + Microsoft.VisualBasic.vbCrLf
                                Next iIndice_1
                                strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf

                            Next dr_Tot
                            strHTML = strHTML + "</TABLE>" + Microsoft.VisualBasic.vbCrLf
                            Try
                                hFile.WriteLine(strHTML)
                                strHTML = ""

                            Catch ex As Exception
                                AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                                System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                se.Close()
                                Exit Sub
                            End Try

                        End If

                        strHTML = strHTML + "<TR" + Microsoft.VisualBasic.vbCrLf
                        strHTML = strHTML + "<TD>" + Microsoft.VisualBasic.vbCrLf
                        strHTML = strHTML + "."
                        strHTML = strHTML + "</TD>" + Microsoft.VisualBasic.vbCrLf
                        strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf

                        strHTML = strHTML + "<TR" + Microsoft.VisualBasic.vbCrLf
                        strHTML = strHTML + "<TD>" + Microsoft.VisualBasic.vbCrLf
                        strHTML = strHTML + "<H1>" + "Totale Prodotto Consegnato Job: " + dgvr_Tot.Cells("RIDI_Job_Descrizione").Value.ToString() + " - " + (fConsumiTotali / 1000.0).ToString("#,#0.0") + " T" + "</H1>" + Microsoft.VisualBasic.vbCrLf
                        strHTML = strHTML + "</TD>" + Microsoft.VisualBasic.vbCrLf
                        strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf

                        strHTML = strHTML + "<H1>" + "</H1>" + Microsoft.VisualBasic.vbCrLf
                        strHTML = strHTML + "<P1>" + "Nr Batch: " + ds_Rep_Int.Tables(0).Rows.Count.ToString() + "</P1>" + Microsoft.VisualBasic.vbCrLf
                        strHTML = strHTML + "<H1>" + "</H1>" + Microsoft.VisualBasic.vbCrLf

                        strHTML = strHTML + "<TR" + Microsoft.VisualBasic.vbCrLf
                        strHTML = strHTML + "<TD>" + Microsoft.VisualBasic.vbCrLf
                        strHTML = strHTML + "."
                        strHTML = strHTML + "</TD>" + Microsoft.VisualBasic.vbCrLf
                        strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf
                    End If

                    'Eseguo le operazioni di calcolo
                    If ds_RicPre_Int.Tables(0).Rows.Count > 0 Then

                        'Tolgo il bitume e filler
                        strSQL = "SELECT SUM(RDP_Net_Kg_Prod) as ""Totale_Net_Kg_Prod"" "
                        strSQL = strSQL + "FROM RepDosPassi INNER JOIN RepDosInt ON RepDosPassi.RDP_RDI_ID = RepDosInt.RDI_ID INNER JOIN NomiProdottiDosaggio ON RepDosPassi.RDP_NPD_Riferimento = NomiProdottiDosaggio.NPD_Riferimento "
                        strSQL = strSQL + "WHERE RDI_RIDI_ID = " + dgvr_Tot.Cells("RIDI_ID").Value.ToString + " AND (RDP_NPD_Riferimento = 'SetAgg_1' OR RDP_NPD_Riferimento = 'SetAgg_2' OR RDP_NPD_Riferimento = 'SetAgg_3' OR RDP_NPD_Riferimento = 'SetAgg_4' OR RDP_NPD_Riferimento = 'SetAgg_5' OR RDP_NPD_Riferimento = 'SetAgg_6' OR RDP_NPD_Riferimento = 'SetAgg_7' OR RDP_NPD_Riferimento = 'SetAgg_8' OR RDP_NPD_Riferimento = 'SetFill_1')"
                        ApriDataSet(strSQL, "", ds_Tot, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")

                        ' Passi Ricetta
                        strSQL = "SELECT RPDP_Nome_Prodotto, RPDP_Set_Perc, NPPD_Umidita_Perc "
                        strSQL = strSQL + "FROM RicPreDosPassi INNER JOIN NomiProdottiPreDosaggio ON RicPreDosPassi.RPDP_Tramoggia = NomiProdottiPreDosaggio.NPPD_Tramoggia AND RicPreDosPassi.RPDP_Nome_Prodotto = NomiProdottiPreDosaggio.NPPD_Nome_Prodotto "
                        strSQL = strSQL + "WHERE RPDP_RPDI_ID = " + ds_RicPre_Int.Tables(0).Rows(0).Item("RPDI_ID").ToString
                        ApriDataSet(strSQL, "", ds_RicPre_Passi, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                        ds_RicPre_Passi.Tables(0).Columns.Add("Consumo Umido")

                        If iSoloTotali = 0 Then

                            fConsumiTotaliUmidita = 0

                            strHTML = strHTML + "<TR" + Microsoft.VisualBasic.vbCrLf
                            strHTML = strHTML + "<TD>" + Microsoft.VisualBasic.vbCrLf
                            strHTML = strHTML + "<H1>" + "Totale Consumi Inerti Umidi Job: " + dgvr_Tot.Cells("RIDI_Job_Descrizione").Value.ToString() + "</H1>" + Microsoft.VisualBasic.vbCrLf
                            strHTML = strHTML + "</TD>" + Microsoft.VisualBasic.vbCrLf
                            strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf

                            strHTML = strHTML + "<TABLE BORDER=1 WIDTH=100%>" + Microsoft.VisualBasic.vbCrLf
                            strHTML = strHTML + "<TR>" + Microsoft.VisualBasic.vbCrLf

                            For Each dc In ds_RicPre_Passi.Tables(0).Columns
                                ' Inserisco le caption
                                strConn = My.Settings.Item("db_ConnectionString").ToString
                                strSQL = "SELECT DCC_Caption FROM [DatiCaptionColonne] WHERE DCC_Nome_Colonna ='" + dc.ColumnName.ToString + "'"
                                Dim cn As New SqlConnection(strConn)
                                cn.Open()
                                Dim cmd As New SqlCommand(strSQL, cn)
                                Dim rdr As SqlDataReader = cmd.ExecuteReader()
                                If rdr.HasRows = True Then
                                    rdr.Read()
                                    dc.Caption = rdr.Item(0).ToString
                                End If
                                rdr.Close()
                                cn.Close()

                                strHTML = strHTML + "<TD>"
                                strHTML = strHTML + dc.Caption.ToString
                                strHTML = strHTML + "</TD>" + Microsoft.VisualBasic.vbCrLf
                            Next dc
                            strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf
                            Try
                                hFile.WriteLine(strHTML)
                                strHTML = ""

                            Catch ex As Exception
                                AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                                System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                se.Close()
                                Exit Sub
                            End Try
                        End If
                        If iSoloTotali = 0 Then
                            For Each dr_RicPre_Passi In ds_RicPre_Passi.Tables(0).Rows
                                strHTML = strHTML + "<TR>" + Microsoft.VisualBasic.vbCrLf
                                b_ds_RicPre_Tot_Row_Add = False
                                For iIndice_1 = 0 To ds_RicPre_Passi.Tables(0).Columns.Count - 1
                                    strHTML = strHTML + "<TD>"
                                    If iIndice_1 = 0 Then
                                        strHTML = strHTML + dr_RicPre_Passi.Item(iIndice_1).ToString()
                                        dr_RicPre_Tot_Select = ds_RicPre_Tot.Tables(0).Select("[Nome Prodotto] = '" + dr_RicPre_Passi.Item(iIndice_1).ToString() + "'")
                                        If dr_RicPre_Tot_Select.Length = 0 Then
                                            b_ds_RicPre_Tot_Row_Add = True
                                        End If
                                        obj_Value(0) = dr_RicPre_Passi.Item(iIndice_1)
                                    ElseIf iIndice_1 = 1 Then
                                        fPercDos = dr_RicPre_Passi.Item(iIndice_1)
                                        strHTML = strHTML + fPercDos.ToString() + " %"
                                    ElseIf iIndice_1 = 2 Then
                                        fConsumiParzialiUmiditaPerc = dr_RicPre_Passi.Item(iIndice_1)
                                        strHTML = strHTML + fConsumiParzialiUmiditaPerc.ToString() + " %"
                                    Else
                                        fConsumiParzialiUmidita = CDbl((((fPercDos * ds_Tot.Tables(0).Rows(0).Item("Totale_Net_Kg_Prod")) / 100.0) / 1000.0))
                                        fConsumiParzialiUmidita = fConsumiParzialiUmidita + ((fConsumiParzialiUmidita * fConsumiParzialiUmiditaPerc) / 100.0)
                                        dr_RicPre_Passi.Item(iIndice_1) = fConsumiParzialiUmidita
                                        strHTML = strHTML + fConsumiParzialiUmidita.ToString("#,#0.0") + " T"
                                        obj_Value(1) = dr_RicPre_Passi.Item(iIndice_1)
                                    End If
                                    strHTML = strHTML + "</TD>" + Microsoft.VisualBasic.vbCrLf
                                Next iIndice_1
                                strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf
                                If b_ds_RicPre_Tot_Row_Add = True Then
                                    ds_RicPre_Tot.Tables(0).Rows.Add(obj_Value)
                                Else
                                    For Each dr_RicPre_Tot In ds_RicPre_Tot.Tables(0).Rows
                                        If dr_RicPre_Tot.Item("Nome Prodotto") = obj_Value(0) Then
                                            fConsumiParzialiUmidita = dr_RicPre_Tot.Item("Consumo Umido")
                                            fConsumiParzialiUmidita = fConsumiParzialiUmidita + obj_Value(1)
                                            dr_RicPre_Tot.Item("Consumo Umido") = fConsumiParzialiUmidita
                                        End If
                                    Next dr_RicPre_Tot
                                    ' fConsumiParzialiUmidita = ds_RicPre_Tot.Tables(0).Rows(0).Item("Consumo Umido")
                                    ' fConsumiParzialiUmidita = fConsumiParzialiUmidita + obj_Value(1)
                                    ' ds_RicPre_Tot.Tables(0).Rows(0).Item("Consumo Umido") = fConsumiParzialiUmidita
                                End If
                                fConsumiTotaliUmidita = fConsumiTotaliUmidita + obj_Value(1)
                                se.NewStep()
                                My.Application.DoEvents()
                            Next dr_RicPre_Passi
                            strHTML = strHTML + "</TABLE>" + Microsoft.VisualBasic.vbCrLf
                            Try
                                hFile.WriteLine(strHTML)
                                strHTML = ""

                            Catch ex As Exception
                                AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                                System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                se.Close()
                                Exit Sub
                            End Try

                            ' Mi serve per il resoconto dei totali
                            strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf
                            strHTML = strHTML + "</TABLE>" + Microsoft.VisualBasic.vbCrLf

                            strHTML = strHTML + "<TR" + Microsoft.VisualBasic.vbCrLf
                            strHTML = strHTML + "<TD>" + Microsoft.VisualBasic.vbCrLf
                            strHTML = strHTML + "<H1>" + "Totale Prodotto Consegnato Inerti Umidi Job: " + dgvr_Tot.Cells("RIDI_Job_Descrizione").Value.ToString() + " - " + fConsumiTotaliUmidita.ToString("#,#0.0") + " T" + "</H1>" + Microsoft.VisualBasic.vbCrLf
                            strHTML = strHTML + "</TD>" + Microsoft.VisualBasic.vbCrLf
                            strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf

                            Try
                                hFile.WriteLine(strHTML)
                                strHTML = ""

                            Catch ex As Exception
                                AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                                System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                se.Close()
                                Exit Sub
                            End Try
                            se.NewStep()
                            My.Application.DoEvents()
                            If se.Annulla = True Then
                                se.Close()
                                Exit Sub
                            End If
                        Else
                            For Each dr_RicPre_Passi In ds_RicPre_Passi.Tables(0).Rows
                                b_ds_RicPre_Tot_Row_Add = False
                                For iIndice_1 = 0 To ds_RicPre_Passi.Tables(0).Columns.Count - 1
                                    If iIndice_1 = 0 Then
                                        dr_RicPre_Tot_Select = ds_RicPre_Tot.Tables(0).Select("[Nome Prodotto] = '" + dr_RicPre_Passi.Item(iIndice_1).ToString() + "'")
                                        If dr_RicPre_Tot_Select.Length = 0 Then
                                            b_ds_RicPre_Tot_Row_Add = True
                                        End If
                                        obj_Value(0) = dr_RicPre_Passi.Item(iIndice_1)
                                    ElseIf iIndice_1 = 1 Then
                                        fPercDos = dr_RicPre_Passi.Item(iIndice_1)
                                    ElseIf iIndice_1 = 2 Then
                                        fConsumiParzialiUmiditaPerc = dr_RicPre_Passi.Item(iIndice_1)
                                    Else
                                        fConsumiParzialiUmidita = CDbl((((fPercDos * ds_Tot.Tables(0).Rows(0).Item("Totale_Net_Kg_Prod")) / 100.0) / 1000.0))
                                        fConsumiParzialiUmidita = fConsumiParzialiUmidita + ((fConsumiParzialiUmidita * fConsumiParzialiUmiditaPerc) / 100.0)
                                        dr_RicPre_Passi.Item(iIndice_1) = fConsumiParzialiUmidita
                                        obj_Value(1) = dr_RicPre_Passi.Item(iIndice_1)
                                    End If
                                Next iIndice_1
                                If b_ds_RicPre_Tot_Row_Add = True Then
                                    ds_RicPre_Tot.Tables(0).Rows.Add(obj_Value)
                                Else
                                    For Each dr_RicPre_Tot In ds_RicPre_Tot.Tables(0).Rows
                                        If dr_RicPre_Tot.Item("Nome Prodotto") = obj_Value(0) Then
                                            fConsumiParzialiUmidita = dr_RicPre_Tot.Item("Consumo Umido")
                                            fConsumiParzialiUmidita = fConsumiParzialiUmidita + obj_Value(1)
                                            dr_RicPre_Tot.Item("Consumo Umido") = fConsumiParzialiUmidita
                                        End If
                                    Next dr_RicPre_Tot
                                End If
                                fConsumiTotaliUmidita = fConsumiTotaliUmidita + obj_Value(1)
                                se.NewStep()
                                My.Application.DoEvents()
                            Next dr_RicPre_Passi
                        End If
                    End If
                End If
            Next dgvr_Tot
        End If

        ' Riassunto dei totali
        ' Verifico che ci sia qualcosa di selezionato
        If dgvsrc.Count > 0 Then
            fConsumiTotali = 0
            bFirstOR = False
            strSQL = "SELECT NPD_ID, SUM(RDP_Net_Kg_Prod) as ""Totale_Net_Kg_Prod"" "
            strSQL = strSQL + "FROM RepDosPassi INNER JOIN RepDosInt ON RepDosPassi.RDP_RDI_ID = RepDosInt.RDI_ID INNER JOIN NomiProdottiDosaggio ON RepDosPassi.RDP_NPD_Riferimento = NomiProdottiDosaggio.NPD_Riferimento "
            strSQL = strSQL + "WHERE "

            For Each dgvr_Tot In dgvsrc
                If bFirstOR = True Then
                    strSQL = strSQL + "OR "
                End If
                strSQL = strSQL + "RDI_RIDI_ID = " + dgvr_Tot.Cells("RIDI_ID").Value.ToString + " "
                bFirstOR = True
            Next dgvr_Tot

            strSQL = strSQL + "GROUP BY(NPD_ID) "
            strSQL = strSQL + "ORDER BY(NPD_ID) "
            ApriDataSet(strSQL, "", ds_Tot, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")

            If ds_Tot.Tables.Count > 0 Then

                strHTML = strHTML + "<DIV align=center>" + Microsoft.VisualBasic.vbCrLf
                strHTML = strHTML + "<H1>" + "</H1>" + Microsoft.VisualBasic.vbCrLf
                strHTML = strHTML + "<H1>" + "</H1>" + Microsoft.VisualBasic.vbCrLf
                strHTML = strHTML + "<H1>" + "</H1>" + Microsoft.VisualBasic.vbCrLf
                strHTML = strHTML + "<H1>" + "---------------------------" + "</H1>" + Microsoft.VisualBasic.vbCrLf

                strHTML = strHTML + "<DIV align=left>" + Microsoft.VisualBasic.vbCrLf
                strHTML = strHTML + "<H1>" + "</H1>" + Microsoft.VisualBasic.vbCrLf
                strHTML = strHTML + "<H1>" + "</H1>" + Microsoft.VisualBasic.vbCrLf
                strHTML = strHTML + "<H1>" + "</H1>" + Microsoft.VisualBasic.vbCrLf
                strHTML = strHTML + "<TR" + Microsoft.VisualBasic.vbCrLf
                strHTML = strHTML + "<TD>" + Microsoft.VisualBasic.vbCrLf
                strHTML = strHTML + "<H1>" + "Totale Consumi" + "</H1>" + Microsoft.VisualBasic.vbCrLf
                strHTML = strHTML + "</TD>" + Microsoft.VisualBasic.vbCrLf
                strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf

                strHTML = strHTML + "<TABLE BORDER=1 WIDTH=100%>" + Microsoft.VisualBasic.vbCrLf
                strHTML = strHTML + "<TR>" + Microsoft.VisualBasic.vbCrLf
                Try
                    hFile.WriteLine(strHTML)
                    strHTML = ""

                Catch ex As Exception
                    AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                    System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                    se.Close()
                    Exit Sub
                End Try

                For Each dc In ds_Tot.Tables(0).Columns
                    ' Inserisco le caption
                    strConn = My.Settings.Item("db_ConnectionString").ToString
                    strSQL = "SELECT DCC_Caption FROM [DatiCaptionColonne] WHERE DCC_Nome_Colonna ='" + dc.ColumnName.ToString + "'"
                    Dim cn As New SqlConnection(strConn)
                    cn.Open()
                    Dim cmd As New SqlCommand(strSQL, cn)
                    Dim rdr As SqlDataReader = cmd.ExecuteReader()
                    If rdr.HasRows = True Then
                        rdr.Read()
                        dc.Caption = rdr.Item(0).ToString
                    End If
                    rdr.Close()
                    cn.Close()

                    strHTML = strHTML + "<TD>"
                    strHTML = strHTML + dc.Caption.ToString
                    strHTML = strHTML + "</TD>" + Microsoft.VisualBasic.vbCrLf
                Next dc
                strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf
                Try
                    hFile.WriteLine(strHTML)
                    strHTML = ""

                Catch ex As Exception
                    AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                    System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                    se.Close()
                    Exit Sub
                End Try

                For Each dr_Tot In ds_Tot.Tables(0).Rows
                    strHTML = strHTML + "<TR>" + Microsoft.VisualBasic.vbCrLf
                    For iIndice_1 = 0 To ds_Tot.Tables(0).Columns.Count - 1
                        strHTML = strHTML + "<TD>"
                        ' Prelevo i nomi dei prodotti
                        If iIndice_1 = 0 Then
                            strHTML = strHTML + GENERICA_DESCRIZIONE_STRINGA("NPD_Nome", "NomiProdottiDosaggio", "NPD_ID", dr_Tot.Item(iIndice_1).ToString, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                        Else
                            fConsumiTotali = fConsumiTotali + dr_Tot.Item(iIndice_1)
                            fConsumiParziali = dr_Tot.Item(iIndice_1)
                            strHTML = strHTML + (fConsumiParziali / 1000.0).ToString("#,#0.0") + " T"
                        End If
                        strHTML = strHTML + "</TD>" + Microsoft.VisualBasic.vbCrLf
                    Next iIndice_1
                    strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf
                    se.NewStep()
                    My.Application.DoEvents()
                Next dr_Tot
                strHTML = strHTML + "</TABLE>" + Microsoft.VisualBasic.vbCrLf
                Try
                    hFile.WriteLine(strHTML)
                    strHTML = ""

                Catch ex As Exception
                    AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                    System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                    se.Close()
                    Exit Sub
                End Try
            End If
            strHTML = strHTML + "<TR" + Microsoft.VisualBasic.vbCrLf
            strHTML = strHTML + "<TD>" + Microsoft.VisualBasic.vbCrLf
            strHTML = strHTML + "<H1>" + "Totale Prodotto Consegnato : " + (fConsumiTotali / 1000.0).ToString("#,#0.0") + " T" + "</H1>" + Microsoft.VisualBasic.vbCrLf
            strHTML = strHTML + "</TD>" + Microsoft.VisualBasic.vbCrLf
            strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf

            'Totale Umido
            If ds_RicPre_Tot.Tables.Count > 0 Then
                fConsumiTotaliUmidita = 0

                strHTML = strHTML + "<DIV align=center>" + Microsoft.VisualBasic.vbCrLf
                strHTML = strHTML + "<H1>" + "</H1>" + Microsoft.VisualBasic.vbCrLf
                strHTML = strHTML + "<H1>" + "</H1>" + Microsoft.VisualBasic.vbCrLf
                strHTML = strHTML + "<H1>" + "</H1>" + Microsoft.VisualBasic.vbCrLf
                strHTML = strHTML + "<H1>" + "---------------------------" + "</H1>" + Microsoft.VisualBasic.vbCrLf

                strHTML = strHTML + "<DIV align=left>" + Microsoft.VisualBasic.vbCrLf
                strHTML = strHTML + "<H1>" + "</H1>" + Microsoft.VisualBasic.vbCrLf
                strHTML = strHTML + "<H1>" + "</H1>" + Microsoft.VisualBasic.vbCrLf
                strHTML = strHTML + "<H1>" + "</H1>" + Microsoft.VisualBasic.vbCrLf
                strHTML = strHTML + "<TR" + Microsoft.VisualBasic.vbCrLf
                strHTML = strHTML + "<TD>" + Microsoft.VisualBasic.vbCrLf
                strHTML = strHTML + "<H1>" + "Totale Consumi Inerti Umidi" + "</H1>" + Microsoft.VisualBasic.vbCrLf
                strHTML = strHTML + "</TD>" + Microsoft.VisualBasic.vbCrLf
                strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf

                strHTML = strHTML + "<TABLE BORDER=1 WIDTH=100%>" + Microsoft.VisualBasic.vbCrLf
                strHTML = strHTML + "<TR>" + Microsoft.VisualBasic.vbCrLf

                For Each dc In ds_RicPre_Tot.Tables(0).Columns
                    ' Inserisco le caption
                    strConn = My.Settings.Item("db_ConnectionString").ToString
                    strSQL = "SELECT DCC_Caption FROM [DatiCaptionColonne] WHERE DCC_Nome_Colonna ='" + dc.ColumnName.ToString + "'"
                    Dim cn As New SqlConnection(strConn)
                    cn.Open()
                    Dim cmd As New SqlCommand(strSQL, cn)
                    Dim rdr As SqlDataReader = cmd.ExecuteReader()
                    If rdr.HasRows = True Then
                        rdr.Read()
                        dc.Caption = rdr.Item(0).ToString
                    End If
                    rdr.Close()
                    cn.Close()

                    strHTML = strHTML + "<TD>"
                    strHTML = strHTML + dc.Caption.ToString
                    strHTML = strHTML + "</TD>" + Microsoft.VisualBasic.vbCrLf
                Next dc
                strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf
                Try
                    hFile.WriteLine(strHTML)
                    strHTML = ""

                Catch ex As Exception
                    AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                    System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                    se.Close()
                    Exit Sub
                End Try

                For Each dr_RicPre_Tot In ds_RicPre_Tot.Tables(0).Rows
                    strHTML = strHTML + "<TR>" + Microsoft.VisualBasic.vbCrLf
                    For iIndice_1 = 0 To ds_RicPre_Tot.Tables(0).Columns.Count - 1
                        strHTML = strHTML + "<TD>"
                        ' Prelevo i nomi dei prodotti
                        If iIndice_1 = 0 Then
                            strHTML = strHTML + dr_RicPre_Tot.Item(iIndice_1).ToString()
                        Else
                            fConsumiParzialiUmidita = dr_RicPre_Tot.Item(iIndice_1)
                            fConsumiTotaliUmidita = fConsumiTotaliUmidita + fConsumiParzialiUmidita
                            strHTML = strHTML + fConsumiParzialiUmidita.ToString("#,#0.0") + " T"
                        End If
                        strHTML = strHTML + "</TD>" + Microsoft.VisualBasic.vbCrLf
                    Next iIndice_1
                    strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf
                    se.NewStep()
                    My.Application.DoEvents()
                Next dr_RicPre_Tot
                strHTML = strHTML + "</TABLE>" + Microsoft.VisualBasic.vbCrLf
                Try
                    hFile.WriteLine(strHTML)
                    strHTML = ""

                Catch ex As Exception
                    AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                    System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                    se.Close()
                    Exit Sub
                End Try
            End If

            strHTML = strHTML + "<TR" + Microsoft.VisualBasic.vbCrLf
            strHTML = strHTML + "<TD>" + Microsoft.VisualBasic.vbCrLf
            strHTML = strHTML + "<H1>" + "Totale Prodotto Consegnato Inerti Umidi: " + fConsumiTotaliUmidita.ToString("#,#0.0") + " T" + "</H1>" + Microsoft.VisualBasic.vbCrLf
            strHTML = strHTML + "</TD>" + Microsoft.VisualBasic.vbCrLf
            strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf
        End If

        ' Fine documento
        strHTML = strHTML + "<DIV align=center>" + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + "<H1>" + "</H1>" + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + "<H1>" + "</H1>" + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + "<IMG src=""" + My.Application.Info.DirectoryPath.ToString() + "\LogoBottom.gif"" align=""center""> " + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + "<H1>" + "</H1>" + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + "<P1>" + "Fine documento" + "</P1>" + Microsoft.VisualBasic.vbCrLf

        strHTML = strHTML + "<DIV align=left>" + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + "<H1>" + "</H1>" + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + "<A href=""http://www.consulenzeperizie.it/"">Developed by Pretolesi Riccardo - www.consulenzeperizie.it</A>" + Microsoft.VisualBasic.vbCrLf

        strHTML = strHTML + "</BODY>" + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + "</HTML>" + Microsoft.VisualBasic.vbCrLf

        Try
            hFile.WriteLine(strHTML)
            strHTML = ""

        Catch ex As Exception
            AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
            System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            se.Close()
            Exit Sub
        End Try

        Try
            hFile.Close()
            se.Close()
            Process.Start(strNomeFile)
        Catch ex As Exception
            AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
            System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
        End Try
    End Sub

End Class
