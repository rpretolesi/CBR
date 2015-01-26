Imports System.Data
Imports System.Data.SqlClient

Public Class BaseGriglia

    Protected m_strSQL As String
    Protected m_strFieldSelect As String
    Protected m_strFieldFrom As String
    Protected m_strFieldAux As String
    Protected m_strFieldWhereDateTime As String
    Protected m_strFieldOrderBy As String
    Protected m_ldLoginData As LoginData
    Protected m_sdiFormIcon As Icon
    Protected m_strFormTitle As String
    Protected m_strConnStringParamName As String

    Protected m_ds As New DataSet
    Protected m_bs As New BindingSource

    Protected Overridable Sub BaseGriglia_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Me.DesignMode = False Then
            Me.Text = m_strFormTitle
            Me.Icon = m_sdiFormIcon

            Dim ts As New TimeSpan(23, 59, 59)

            Me.ToolStripButton_Nuovo.Enabled = False
            Me.ToolStripButton_Elimina.Enabled = False
            Me.ToolStripButton_Modifica.Enabled = False
            Me.ToolStripButton_Salva.Enabled = False
            Me.ToolStripButton_Annulla.Enabled = False

            If m_strFieldWhereDateTime = "" Then
                Label_DataInizio.Visible = False
                DateTimePicker_Inizio.Visible = False

                Label_DataFine.Visible = False
                DateTimePicker_Fine.Visible = False
            End If

            DateTimePicker_Inizio.Value = Now().Date

            DateTimePicker_Fine.Value = DateTimePicker_Inizio.Value.Add(ts)

            CostruisciSQL()
            EseguiSQL()
            PopolaGriglia()
        End If

    End Sub

    Protected Overridable Sub DateTimePicker_Inizio_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DateTimePicker_Inizio.ValueChanged
        If Me.DesignMode = False Then
            CostruisciSQL()
            EseguiSQL()
            PopolaGriglia()
        End If
    End Sub

    Protected Overridable Sub DateTimePicker_Fine_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DateTimePicker_Fine.ValueChanged
        If Me.DesignMode = False Then
            CostruisciSQL()
            EseguiSQL()
            PopolaGriglia()
        End If
    End Sub

    Protected Sub CostruisciSQL()
        If Me.DesignMode = False Then
            Dim ts As New TimeSpan(1, 0, 0, 0)
            Dim dt As New DateTime
            dt = DateTimePicker_Inizio.Value.Subtract(ts)
            If m_strFieldWhereDateTime = "" Then
                m_strSQL = "SELECT " + m_strFieldSelect + " FROM " + m_strFieldFrom + m_strFieldAux
            Else
                If m_strFieldFrom.Contains("GROUP BY") = True Then
                    m_strSQL = "SELECT " + m_strFieldSelect + " FROM " + m_strFieldFrom + m_strFieldAux + " HAVING " + m_strFieldWhereDateTime + " BETWEEN '" + DateTimePicker_Inizio.Value.ToString("G", Globalization.CultureInfo.CreateSpecificCulture("de-DE")) + "' AND '" + DateTimePicker_Fine.Value.ToString("G", Globalization.CultureInfo.CreateSpecificCulture("de-DE")) + "' ORDER BY " + m_strFieldOrderBy
                Else
                    m_strSQL = "SELECT " + m_strFieldSelect + " FROM " + m_strFieldFrom + m_strFieldAux + " WHERE " + m_strFieldWhereDateTime + " BETWEEN '" + DateTimePicker_Inizio.Value.ToString("G", Globalization.CultureInfo.CreateSpecificCulture("de-DE")) + "' AND '" + DateTimePicker_Fine.Value.ToString("G", Globalization.CultureInfo.CreateSpecificCulture("de-DE")) + "' ORDER BY " + m_strFieldOrderBy
                End If
            End If
        End If
    End Sub

    Protected Sub EseguiSQL()
        If Me.DesignMode = False Then

            Try
                Dim strSQL, strConn As String
                Dim da As New SqlDataAdapter(m_strSQL, My.Settings.Item(m_strConnStringParamName).ToString)
                m_ds.Clear()
                da.Fill(m_ds)

                If m_ds.Tables(0).Columns.Count > 0 Then
                    For Each dc As DataColumn In m_ds.Tables(0).Columns
                        ' Inserisco le caption
                        strConn = My.Settings.Item(m_strConnStringParamName).ToString
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

                    Next dc

                End If

            Catch ex As Exception
                AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, m_ldLoginData, m_strFormTitle, m_strConnStringParamName)
                System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, m_strFormTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End Try

            ' Lo associo
            Try
                Me.m_bs.DataSource = Me.m_ds.Tables(0)

            Catch ex As Exception
                AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, m_ldLoginData, m_strFormTitle, m_strConnStringParamName)
                System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, m_strFormTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End Try

            Try
                If m_ds.Tables(0).Rows.Count = 0 Then
                    Me.ToolStripButton_Elimina.Enabled = False
                    Me.ToolStripButton_Modifica.Enabled = False
                Else
                    Me.ToolStripButton_Elimina.Enabled = True
                    Me.ToolStripButton_Modifica.Enabled = True
                End If
            Catch ex As Exception
                AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, m_ldLoginData, m_strFormTitle, m_strConnStringParamName)
                System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, m_strFormTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End Try
        End If
    End Sub

    Protected Sub PopolaGriglia()
        If Me.DesignMode = False Then
            Dim iIndice_1 As Integer
            Dim iTotalRecord As Integer

            iTotalRecord = 0
            Try

                If m_ds.Tables.Count > 0 Then

                    ' Preparo i dati
                    With Me.DataGridView

                        ' Inserisco i dati
                        .ReadOnly = True
                        .SelectionMode = DataGridViewSelectionMode.FullRowSelect
                        .RowHeadersVisible = True
                        .AutoGenerateColumns = True

                        .DataSource = m_bs.DataSource

                        For iIndice_1 = 0 To .Columns.Count - 1
                            .Columns(iIndice_1).Visible = True
                        Next iIndice_1

                        For iIndice_1 = 0 To .Columns.Count - 1
                            If m_ds.Tables(0).Columns(iIndice_1).ColumnName.ToString = m_ds.Tables(0).Columns(iIndice_1).Caption.ToString Then
                                .Columns(iIndice_1).Visible = False
                            Else
                                .Columns(iIndice_1).HeaderText = m_ds.Tables(0).Columns(iIndice_1).Caption
                            End If
                        Next iIndice_1

                    End With
                    iTotalRecord = m_ds.Tables(0).Rows.Count
                End If

            Catch ex As Exception
                AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, m_ldLoginData, m_strFormTitle, m_strConnStringParamName)
                System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, m_strFormTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End Try

            ' Imposto il numero totale di record
            TextBox_Nr_Record.Text = iTotalRecord.ToString() + " / 0"
        End If
    End Sub

    Protected Overridable Sub BaseGriglia_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.SizeChanged
        If Me.DesignMode = False Then
            Static bOneShot As Boolean
            Static szFormOriginalSize As Size

            Static szDataGridViewOriginalSize As Size
            Static ptDataGridViewOriginalLocation As Point

            Static ptLabel_DataInizioOriginalLocation As Point
            Static ptDateTimePicker_InizioOriginalLocation As Point
            Static ptLabel_DataFineOriginalLocation As Point
            Static ptDateTimePicker_FineOriginalLocation As Point

            Static ptLabel_Nr_RecordOriginalLocation As Point
            Static ptTextBox_Nr_RecordOriginalLocation As Point

            If bOneShot = False Then
                bOneShot = True
                szFormOriginalSize = Me.Size

                szDataGridViewOriginalSize = Me.DataGridView.Size

                ptDataGridViewOriginalLocation = Me.DataGridView.Location

                ptLabel_DataInizioOriginalLocation = Me.Label_DataInizio.Location
                ptDateTimePicker_InizioOriginalLocation = DateTimePicker_Inizio.Location

                ptLabel_DataFineOriginalLocation = Me.Label_DataFine.Location
                ptDateTimePicker_FineOriginalLocation = Me.DateTimePicker_Fine.Location

                ptLabel_Nr_RecordOriginalLocation = Me.Label_Nr_Record.Location
                ptTextBox_Nr_RecordOriginalLocation = Me.TextBox_Nr_Record.Location
            End If

            Me.DataGridView.Height = szDataGridViewOriginalSize.Height - (szFormOriginalSize.Height - Me.Height)
            Me.DataGridView.Width = szDataGridViewOriginalSize.Width - (szFormOriginalSize.Width - Me.Width)
            'Me.DataGridViewNT.Top = ptDataGridViewNTOriginalLocation.Y - (szFormOriginalSize.Height - Me.Height)

            Me.Label_DataInizio.Top = ptLabel_DataInizioOriginalLocation.Y - ((szFormOriginalSize.Height - Me.Height))
            DateTimePicker_Inizio.Top = ptDateTimePicker_InizioOriginalLocation.Y - ((szFormOriginalSize.Height - Me.Height))

            Me.Label_DataFine.Top = ptLabel_DataFineOriginalLocation.Y - ((szFormOriginalSize.Height - Me.Height))
            Me.DateTimePicker_Fine.Top = ptDateTimePicker_FineOriginalLocation.Y - ((szFormOriginalSize.Height - Me.Height))

            Me.Label_Nr_Record.Top = ptLabel_Nr_RecordOriginalLocation.Y - ((szFormOriginalSize.Height - Me.Height))
            Me.TextBox_Nr_Record.Top = ptTextBox_Nr_RecordOriginalLocation.Y - ((szFormOriginalSize.Height - Me.Height))
        End If
    End Sub

    Protected Overridable Sub DataGridView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridView.Click
        If Me.DesignMode = False Then
            Dim mea As System.Windows.Forms.MouseEventArgs
            mea = e
            If mea.Button = Windows.Forms.MouseButtons.Right Then
                ContextMenuStrip_1.Show(sender.MousePosition.X, sender.MousePosition.Y)
            End If
        End If
    End Sub

    Protected Sub DataGridViewNT_ColumnHeaderMouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles DataGridView.ColumnHeaderMouseDoubleClick
        If Me.DesignMode = False Then
            DataGridView.Columns(e.ColumnIndex).Visible = False
        End If
    End Sub

    Protected Overridable Sub DataGridView_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridView.SelectionChanged
        If Me.DesignMode = False Then
            Try
                If DataGridView.SelectedRows.Count > 0 Then
                    If DataGridView.SelectedRows(0).Index >= 0 Then

                        If m_ds.Tables(0).Rows.Count > DataGridView.SelectedRows(0).Index Then
                            Me.m_bs.Position = DataGridView.SelectedRows(0).Index
                        End If

                    End If
                End If
            Catch ex As Exception
                AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, m_ldLoginData, m_strFormTitle, m_strConnStringParamName)
                System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, m_strFormTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End Try

            ' Imposto il numero totale di record
            TextBox_Nr_Record.Text = m_ds.Tables(0).Rows.Count.ToString() + " / " + Me.DataGridView.SelectedRows.Count.ToString()
        End If
    End Sub

    Overloads Function ShowDialog(ByVal owner As System.Windows.Forms.IWin32Window, ByVal strFieldSelect As String, ByVal strFieldFrom As String, ByVal strFieldAux As String, ByVal strFieldWhereDateTime As String, ByVal strFieldOrderBy As String, ByVal ldLoginData As LoginData, ByVal sdiFormIcon As Icon, ByVal strFormTitle As String, ByVal strConnStringParamName As String) As Windows.Forms.DialogResult
        If Me.DesignMode = False Then
            m_strFieldSelect = strFieldSelect
            m_strFieldFrom = strFieldFrom
            m_strFieldAux = strFieldAux
            m_strFieldWhereDateTime = strFieldWhereDateTime
            m_strFieldOrderBy = strFieldOrderBy
            m_ldLoginData = ldLoginData
            m_sdiFormIcon = sdiFormIcon
            m_strFormTitle = strFormTitle
            m_strConnStringParamName = strConnStringParamName
        End If
        Return MyBase.ShowDialog(owner)
    End Function

    Overloads Sub Show(ByVal strFieldSelect As String, ByVal strFieldFrom As String, ByVal strFieldAux As String, ByVal strFieldWhereDateTime As String, ByVal strFieldOrderBy As String, ByVal ldLoginData As LoginData, ByVal sdiFormIcon As Icon, ByVal strFormTitle As String, ByVal strConnStringParamName As String)
        If Me.DesignMode = False Then
            m_strFieldSelect = strFieldSelect
            m_strFieldFrom = strFieldFrom
            m_strFieldAux = strFieldAux
            m_strFieldWhereDateTime = strFieldWhereDateTime
            m_strFieldOrderBy = strFieldOrderBy
            m_ldLoginData = ldLoginData
            m_sdiFormIcon = sdiFormIcon
            m_strFormTitle = strFormTitle
            m_strConnStringParamName = strConnStringParamName
        End If
        MyBase.Show()
    End Sub

    Private Sub ToolStripButton_Stampa_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_Stampa.Click
        If Me.DesignMode = False Then
            Me.Enabled = False
            Stampa()
            Me.Enabled = True
        End If
    End Sub

    Private Sub ToolStripButtonSelezionaTutto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButtonSelezionaTutto.Click
        If Me.DesignMode = False Then
            Me.DataGridView.SelectAll()
        End If
    End Sub

    Protected Overridable Sub ContextMenuStrip_ItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles ContextMenuStrip_1.ItemClicked
        If Me.DesignMode = False Then
            If e.ClickedItem.Name = "StampaToolStripMenuItem" Then
                Me.Enabled = False
                Stampa()
                Me.Enabled = True
            End If
        End If
    End Sub

    Private Sub Stampa()
        Dim iIndice_1 As Integer
        Dim iIndice_2 As Integer
        Dim strHTML As String
        Dim strNomeFile As String
        Dim iPrintedRecord As Integer

        Dim hFile As System.IO.StreamWriter

        'Stato Avanzamento
        Dim se As New StatoElab
        se.Show(Me)

        strHTML = ""
        'strNomeFile = My.Application.Info.DirectoryPath.ToString() + "\" + Me.Text + ".html"
        strNomeFile = Environment.GetEnvironmentVariables().Item("TEMP").ToString() + "\" + Me.Text + ".html"
        Try
            hFile = My.Computer.FileSystem.OpenTextFileWriter(strNomeFile, False)
            hFile.WriteLine(strHTML)
            'My.Computer.FileSystem.WriteAllText(strNomeFile, strHTML, False)
            strHTML = ""

        Catch ex As Exception
            AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, m_ldLoginData, m_strFormTitle, m_strConnStringParamName)
            System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, m_strFormTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Exit Sub
        End Try

        strHTML = "<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0//EN""" + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + """http://www.w3.org/TR/REC-html140/strict.dtd"">" + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + "<HTML>" + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + "<HEAD>" + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + "<TITLE>" + Me.Text + "</TITLE>" + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + "</HEAD>" + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + "<BODY>" + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + "<H1>" + Me.Text + "</H1>" + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + "<H1>" + "</H1>" + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + "<P1>" + "Elaborato il: " + Date.Now.ToString + " - Da: " + m_ldLoginData.strNome + "</P1>" + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + "<H1>" + "</H1>" + Microsoft.VisualBasic.vbCrLf

        ' Verifico che ci sia qualcosa di selezionato
        iPrintedRecord = 0
        If Not Me.DataGridView.CurrentRow Is Nothing Then
            strHTML = strHTML + "<TABLE BORDER=1 WIDTH=100%>" + Microsoft.VisualBasic.vbCrLf

            strHTML = strHTML + "<TR>" + Microsoft.VisualBasic.vbCrLf
            For iIndice_1 = 0 To Me.DataGridView.ColumnCount - 1
                If Me.DataGridView.Columns(iIndice_1).Visible = True Then
                    strHTML = strHTML + "<TD>"
                    strHTML = strHTML + Me.DataGridView.Columns(iIndice_1).HeaderText.ToString
                    strHTML = strHTML + "</TD>" + Microsoft.VisualBasic.vbCrLf
                End If
            Next iIndice_1
            strHTML = strHTML + "</TR>" + Microsoft.VisualBasic.vbCrLf
            If Me.DataGridView.SelectedRows.Count > 0 Then
                For iIndice_1 = 0 To Me.DataGridView.SelectedRows.Count - 1
                    strHTML = strHTML + "<TR>" + Microsoft.VisualBasic.vbCrLf
                    For iIndice_2 = 0 To Me.DataGridView.ColumnCount - 1
                        If Me.DataGridView.Columns(iIndice_2).Visible = True Then
                            strHTML = strHTML + "<TD>"
                            strHTML = strHTML + Me.DataGridView.SelectedRows.Item((Me.DataGridView.SelectedRows.Count - 1) - iIndice_1).Cells(iIndice_2).Value.ToString
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
                        Exit Sub
                    End Try
                    se.NewStep()
                    My.Application.DoEvents()
                Next iIndice_1
                iPrintedRecord = Me.DataGridView.SelectedRows.Count.ToString()
            End If

            strHTML = strHTML + "</TABLE>" + Microsoft.VisualBasic.vbCrLf
        End If

        strHTML = strHTML + "<H1>" + "</H1>" + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + "<P1>" + "Nr Record: " + iPrintedRecord.ToString() + "</P1>" + Microsoft.VisualBasic.vbCrLf

        strHTML = strHTML + "<DIV align=center>" + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + "<H1>" + "</H1>" + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + "<H1>" + "</H1>" + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + "<IMG src=""" + My.Application.Info.DirectoryPath.ToString() + "\LogoBottom.gif"" align=""center""> " + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + "<H1>" + "</H1>" + Microsoft.VisualBasic.vbCrLf
        strHTML = strHTML + "<P1>" + "Fine Documento." + "</P1>" + Microsoft.VisualBasic.vbCrLf
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
            AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, m_ldLoginData, m_strFormTitle, m_strConnStringParamName)
            System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, m_strFormTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Exit Sub
        End Try

        Try
            hFile.Close()
            se.Close()
            Process.Start(strNomeFile)
        Catch ex As Exception
            AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, m_ldLoginData, m_strFormTitle, m_strConnStringParamName)
            System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, m_strFormTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop)
        End Try

    End Sub

    ' Eventi binding navigator
    Protected Overridable Sub ToolStripButton_Nuovo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_Nuovo.Click
        If Me.DesignMode = False Then
            ' Aggiorno
            Me.m_bs.Position = -1

            Me.ToolStripButton_Nuovo.Enabled = False
            Me.ToolStripButton_Elimina.Enabled = False
            Me.ToolStripButton_Modifica.Enabled = False
            Me.ToolStripButton_Salva.Enabled = True
            Me.ToolStripButton_Annulla.Enabled = True

            Me.DataGridView.Enabled = False

            AbilitaControlli(True)
        End If
    End Sub

    Protected Overridable Sub ToolStripButton_Elimina_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_Elimina.Click
        If Me.DesignMode = False Then
            Me.ToolStripButton_Nuovo.Enabled = True
            Me.ToolStripButton_Elimina.Enabled = True
            Me.ToolStripButton_Modifica.Enabled = True
            Me.ToolStripButton_Salva.Enabled = False
            Me.ToolStripButton_Annulla.Enabled = False

            Me.DataGridView.Enabled = True

            AbilitaControlli(False)
        End If
    End Sub

    Protected Overridable Sub ToolStripButton_Modifica_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_Modifica.Click
        If Me.DesignMode = False Then
            Me.ToolStripButton_Nuovo.Enabled = False
            Me.ToolStripButton_Elimina.Enabled = False
            Me.ToolStripButton_Modifica.Enabled = False
            Me.ToolStripButton_Salva.Enabled = True
            Me.ToolStripButton_Annulla.Enabled = True

            Me.DataGridView.Enabled = False

            AbilitaControlli(True)
        End If
    End Sub

    Protected Overridable Sub ToolStripButton_Salva_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_Salva.Click
        If Me.DesignMode = False Then
            ' Aggiorno
            Me.m_bs.Position = -1

            Me.ToolStripButton_Nuovo.Enabled = True
            Me.ToolStripButton_Elimina.Enabled = True
            Me.ToolStripButton_Modifica.Enabled = True
            Me.ToolStripButton_Salva.Enabled = False
            Me.ToolStripButton_Annulla.Enabled = False

            Me.DataGridView.Enabled = True

            AbilitaControlli(False)
        End If
    End Sub

    Protected Overridable Sub ToolStripButton_Annulla_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_Annulla.Click
        If Me.DesignMode = False Then
            ' Aggiorno
            Me.m_bs.Position = -1

            Me.ToolStripButton_Nuovo.Enabled = True
            Me.ToolStripButton_Elimina.Enabled = True
            Me.ToolStripButton_Modifica.Enabled = True
            Me.ToolStripButton_Salva.Enabled = False
            Me.ToolStripButton_Annulla.Enabled = False

            Me.DataGridView.Enabled = True

            AbilitaControlli(False)
        End If
    End Sub

    Protected Overridable Sub AbilitaControlli(ByVal bAbilitaControlli As Boolean)
        If Me.DesignMode = False Then

        End If
    End Sub
End Class