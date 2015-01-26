Imports System.Data
Imports System.Data.SqlClient

Public Class GestNomiDos

    Protected Overrides Sub BaseGriglia_Load(ByVal sender As Object, ByVal e As System.EventArgs)
        MyBase.BaseGriglia_Load(sender, e)
        If Me.DesignMode = False Then
            Me.Label_NPD_Riferimento.Text = m_ds.Tables(0).Columns(1).Caption
            Me.Label_NPD_Nome.Text = m_ds.Tables(0).Columns(2).Caption

            Me.TextBox_NPD_Riferimento.DataBindings.Add(New Binding("Text", MyBase.m_bs, "NPD_Riferimento", True))
            Me.TextBox_NPD_Nome.DataBindings.Add(New Binding("Text", MyBase.m_bs, "NPD_Nome", True))

        End If
    End Sub
    Protected Overrides Sub BaseGriglia_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs)
        MyBase.BaseGriglia_FormClosed(sender, e)

        Me.TextBox_NPD_Riferimento.DataBindings.Clear()
        Me.TextBox_NPD_Nome.DataBindings.Clear()
    End Sub

    Protected Overrides Sub ToolStripButton_Nuovo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Protected Overrides Sub ToolStripButton_Elimina_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Protected Overrides Sub ToolStripButton_Salva_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MyBase.ToolStripButton_Salva_Click(sender, e)
        AddLogEvent(LOG_OK, "Esecuzione Aggiornamento Nomi Prodotti Dosaggio.", Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")

        Try
            Dim strConn, strSQL As String
            Dim dtDateTime As Date = Date.Now

            strConn = My.Settings.Item("db_ConnectionString")
            Dim cn As New SqlConnection(strConn)
            cn.Open()

            For Each dr As DataRow In m_ds.Tables(0).Rows
                If dr.RowState = DataRowState.Modified Then

                    strSQL = "UPDATE [NomiProdottiDosaggio] "
                    strSQL = strSQL + "SET NPD_Nome = @NPD_Nome "
                    strSQL = strSQL + "WHERE NPD_Riferimento = @NPD_Riferimento"

                    Dim cmdUpdate As New SqlCommand(strSQL, cn)

                    cmdUpdate.Parameters.AddWithValue("@NPD_Riferimento", dr.Item("NPD_Riferimento"))
                    cmdUpdate.Parameters.AddWithValue("@NPD_Nome", dr.Item("NPD_Nome"))

                    Try
                        If cmdUpdate.ExecuteNonQuery() > 0 Then
                            AddLogEvent(LOG_OK, "Aggiornamento Nomi Prodotti Dosaggio OK.", Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                            System.Windows.Forms.MessageBox.Show("Aggiornamento Nomi Prodotti Dosaggio OK.", Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Else
                            AddLogEvent(LOG_ERROR, "Errore nell'Aggiornamento Nomi Prodotti Dosaggio.", Main.m_ldLoginData, Main.APP_TITLE, "db_ConnectionString")
                            System.Windows.Forms.MessageBox.Show("Errore nell'Aggiornamento Nomi Prodotti Dosaggio.", Main.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Stop)
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
        Me.TextBox_NPD_Nome.Enabled = bAbilitaControlli
    End Sub

End Class
