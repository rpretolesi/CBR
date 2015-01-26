Imports System.Data.SqlClient

Public Class GestRicPreDos

    Public Sub New()

        ' Chiamata richiesta da Progettazione Windows Form.
        InitializeComponent()

        ' Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent().
        Dim tsi As System.Windows.Forms.ToolStripItem
        Dim tss As New System.Windows.Forms.ToolStripSeparator

        ' Separatore
        ContextMenuStrip_1.Items.Add(tss)

        tsi = ContextMenuStrip_1.Items.Add("Stampa Ricetta Predosaggio")
        tsi.Name = "StampaRicettaPredosaggioToolStripMenuItem"

    End Sub

    Protected Overrides Sub ContextMenuStrip_ItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs)
        If Me.DesignMode = False Then
            If e.ClickedItem.Name = "StampaRicettaPredosaggioToolStripMenuItem" Then
                Me.Enabled = False
                StampaRicettaPredosaggio()
                Me.Enabled = True
            End If
        End If
        MyBase.ContextMenuStrip_ItemClicked(sender, e)
    End Sub

    Private Sub StampaRicettaPredosaggio()
        Dim iIndice_1 As Integer
        Dim strHTML As String
        Dim strNomeFile As String

        Dim strSQL, strConn As String
        Dim ds_RicPre_Int As New DataSet
        Dim ds_RicPre_Passi As New DataSet
        Dim dc As DataColumn
        Dim dr_Int As DataRow
        Dim dr_Passi As DataRow

        Dim dgvr As Windows.Forms.DataGridViewRow

        ' Stato avanzamento
        Dim se As New StatoElab
        se.Show(Me)

        strHTML = "<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0//EN""" + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + """http://www.w3.org/TR/REC-html140/strict.dtd"">" + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + "<HTML>" + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + "<HEAD>" + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + "<TITLE>" + Main.APP_TITLE + " - Ricetta Di PreDosaggio" + "</TITLE>" + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + "</HEAD>" + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + "<BODY>" + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + "<H1>" + Main.APP_TITLE + " - Ricetta Di PreDosaggio" + "</H1>" + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + "<H1>" + "</H1>" + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + "<H1>" + "Elaborato il: " + Date.Now.ToString + "</H1>" + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + "<H1>" + "</H1>" + Microsoft.VisualBasic.vbCrLf

        If Me.DataGridView.SelectedRows.Count > 0 Then
            For Each dgvr In Me.DataGridView.SelectedRows

                ' Prelevo l'ID da stampare
                strSQL = "SELECT RPDI_ID, RPDI_Nome, RPDI_Descrizione, RPDI_PredRicInV, RPDI_PrRicSeEl, RPDI_EsclBrImp, RPDI_TmpRicPrd, RPDI_MRSuAnEdEl "
                strSQL = strSQL + "FROM RicPreDosInt "
                strSQL = strSQL + "WHERE RPDI_ID = " + dgvr.Cells("RPDI_ID").Value.ToString()
                ApriDataSet(strSQL, "", ds_RicPre_Int, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")

                ' Intestazione Ricetta
                strHTML = strHTML + "<DIV align=center>" + Microsoft.VisualBasic.vbCrLf
                strHTML = strHTML + "<H1>" + "</H1>" + Microsoft.VisualBasic.vbCrLf
                strHTML = strHTML + "<H1>" + "</H1>" + Microsoft.VisualBasic.vbCrLf
                strHTML = strHTML + "<H1>" + "---------------------------" + "</H1>" + Microsoft.VisualBasic.vbCrLf
                strHTML = strHTML + "<H1>" + "</H1>" + Microsoft.VisualBasic.vbCrLf

                strHTML = strHTML + "<DIV align=left>" + Microsoft.VisualBasic.vbCrLf
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

                    strHTML = strHTML + "<TD>"
                    strHTML = strHTML + dc.Caption.ToString
                    strHTML = strHTML + "</TD>" + Microsoft.VisualBasic.vbCrLf

                    se.NewStep()
                    My.Application.DoEvents()
                Next dc
                strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf
                For Each dr_Int In ds_RicPre_Int.Tables(0).Rows
                    strHTML = strHTML + "<TR>" + Microsoft.VisualBasic.vbCrLf
                    For iIndice_1 = 0 To ds_RicPre_Int.Tables(0).Columns.Count - 1
                        strHTML = strHTML + "<TD>"
                        strHTML = strHTML + dr_Int.Item(iIndice_1).ToString
                        strHTML = strHTML + "</TD>" + Microsoft.VisualBasic.vbCrLf
                    Next iIndice_1
                    strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf
                    se.NewStep()
                    My.Application.DoEvents()
                Next dr_Int
                strHTML = strHTML + "</TABLE>" + Microsoft.VisualBasic.vbCrLf

                ' Passi Ricetta
                strSQL = "SELECT RPDP_ID, RPDP_Nome_Prodotto, RPDP_Set_Perc, RPDP_Set_Toll, RPDP_Set_Rit_Start, RPDP_Set_Rit_Stop, RPDP_Set_Ponderale "
                strSQL = strSQL + "FROM RicPreDosPassi "
                strSQL = strSQL + "WHERE RPDP_RPDI_ID = " + dr_Int.Item("RPDI_ID").ToString
                ApriDataSet(strSQL, "", ds_RicPre_Passi, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")

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
                    se.NewStep()
                    My.Application.DoEvents()
                Next dc
                strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf

                For Each dr_Passi In ds_RicPre_Passi.Tables(0).Rows
                    strHTML = strHTML + "<TR>" + Microsoft.VisualBasic.vbCrLf
                    For iIndice_1 = 0 To ds_RicPre_Passi.Tables(0).Columns.Count - 1
                        strHTML = strHTML + "<TD>"
                        strHTML = strHTML + dr_Passi.Item(iIndice_1).ToString
                        strHTML = strHTML + "</TD>" + Microsoft.VisualBasic.vbCrLf
                    Next iIndice_1
                    strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf
                    se.NewStep()
                    My.Application.DoEvents()
                Next dr_Passi

                strHTML = strHTML + "</TABLE>" + Microsoft.VisualBasic.vbCrLf

                se.NewStep()
                My.Application.DoEvents()
            Next dgvr
        End If

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

        'strNomeFile = My.Application.Info.DirectoryPath.ToString() + "\" + Main.APP_TITLE + " - Rapporto Di Dosaggio" + ".html"
        strNomeFile = Environment.GetEnvironmentVariables().Item("TEMP").ToString() + "\" + Main.APP_TITLE + " - Rapporto Di Dosaggio" + ".html"
        Try
            My.Computer.FileSystem.WriteAllText(strNomeFile, strHTML, False)
            Try
                se.Close()
                Process.Start(strNomeFile)
            Catch ex As Exception
                AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                Exit Sub
            End Try
        Catch ex As Exception
            AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
            System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            se.Close()
            Exit Sub
        End Try
    End Sub

End Class
