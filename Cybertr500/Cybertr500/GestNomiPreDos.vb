Imports System.Data
Imports System.Data.SqlClient

Public Class GestNomiPreDos

    Protected Overrides Sub BaseGriglia_Load(ByVal sender As Object, ByVal e As System.EventArgs)
        MyBase.BaseGriglia_Load(sender, e)
        If Me.DesignMode = False Then
            Me.TextBox_NPPD_Tramoggia.Text = m_ds.Tables(0).Columns(1).Caption
            Me.TextBox_NPPD_Nome_Prodotto.Text = m_ds.Tables(0).Columns(2).Caption
            Me.TextBox_NPPD_Umidita_Perc.Text = m_ds.Tables(0).Columns(3).Caption

            Me.TextBox_NPPD_Tramoggia.DataBindings.Add(New Binding("Text", MyBase.m_bs, "NPPD_Tramoggia", True))
            Me.TextBox_NPPD_Nome_Prodotto.DataBindings.Add(New Binding("Text", MyBase.m_bs, "NPPD_Nome_Prodotto", True))
            Me.TextBox_NPPD_Umidita_Perc.DataBindings.Add(New Binding("Text", MyBase.m_bs, "NPPD_Umidita_Perc", True))

        End If
    End Sub
    Protected Overrides Sub BaseGriglia_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs)
        MyBase.BaseGriglia_FormClosed(sender, e)

        Me.TextBox_NPPD_Tramoggia.DataBindings.Clear()
        Me.TextBox_NPPD_Nome_Prodotto.DataBindings.Clear()
        Me.TextBox_NPPD_Umidita_Perc.DataBindings.Clear()
    End Sub

    Protected Overrides Sub ToolStripButton_Nuovo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Protected Overrides Sub ToolStripButton_Elimina_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Protected Overrides Sub ToolStripButton_Salva_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MyBase.ToolStripButton_Salva_Click(sender, e)
        AddLogEvent(LOG_OK, "Esecuzione Aggiornamento Umidita' Prodotti Predosaggio.", Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")

        Try
            Dim strConn, strSQL As String
            Dim dtDateTime As Date = Date.Now

            strConn = My.Settings.Item("db_ConnectionString")
            Dim cn As New SqlConnection(strConn)
            cn.Open()

            For Each dr As DataRow In m_ds.Tables(0).Rows
                If dr.RowState = DataRowState.Modified Then

                    strSQL = "UPDATE [NomiProdottiPreDosaggio] "
                    strSQL = strSQL + "SET NPPD_Umidita_Perc = @NPPD_Umidita_Perc "
                    strSQL = strSQL + "WHERE NPPD_Tramoggia = @NPPD_Tramoggia AND NPPD_Nome_Prodotto = @NPPD_Nome_Prodotto"

                    Dim cmdUpdate As New SqlCommand(strSQL, cn)

                    cmdUpdate.Parameters.AddWithValue("@NPPD_Tramoggia", dr.Item("NPPD_Tramoggia"))
                    cmdUpdate.Parameters.AddWithValue("@NPPD_Nome_Prodotto", dr.Item("NPPD_Nome_Prodotto"))
                    cmdUpdate.Parameters.AddWithValue("@NPPD_Umidita_Perc", dr.Item("NPPD_Umidita_Perc"))

                    Try
                        If cmdUpdate.ExecuteNonQuery() > 0 Then
                            AddLogEvent(LOG_OK, "Aggiornamento Umidita' Prodotti PreDosaggio OK.", Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                            System.Windows.Forms.MessageBox.Show("Aggiornamento Umidita' Prodotti PreDosaggio OK.", Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Else
                            AddLogEvent(LOG_ERROR, "Errore nell'Aggiornamento Umidita' Prodotti PreDosaggio.", Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                            System.Windows.Forms.MessageBox.Show("Errore nell'Aggiornamento Umidita' Prodotti PreDosaggio.", Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                        End If
                    Catch ex As Exception
                        AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, m_ldLoginData, m_strFormTitle, m_strConnStringParamName)
                        System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, m_strFormTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                    End Try
                End If
            Next dr

            cn.Close()

        Catch ex As Exception
            AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, m_ldLoginData, m_strFormTitle, m_strConnStringParamName)
            System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, m_strFormTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop)
        End Try

        m_ds.Tables(0).AcceptChanges()

    End Sub

    Protected Overrides Sub ToolStripButton_Annulla_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MyBase.ToolStripButton_Annulla_Click(sender, e)

        m_ds.Tables(0).AcceptChanges()

    End Sub

    Protected Overrides Sub AbilitaControlli(ByVal bAbilitaControlli As Boolean)
        Me.TextBox_NPPD_Umidita_Perc.Enabled = bAbilitaControlli
    End Sub

End Class
