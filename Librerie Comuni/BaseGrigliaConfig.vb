Public Class BaseGrigliaConfig

    Protected Overrides Sub BaseGriglia_Load(ByVal sender As Object, ByVal e As System.EventArgs)
        MyBase.BaseGriglia_Load(sender, e)
        If Me.DesignMode = False Then
            Try
                Me.ToolStripButton_Nuovo.Enabled = True
                If m_ds.Tables(0).Rows.Count > 0 Then
                    Me.ToolStripButton_Elimina.Enabled = True
                    Me.ToolStripButton_Modifica.Enabled = True
                Else
                    Me.ToolStripButton_Elimina.Enabled = False
                    Me.ToolStripButton_Modifica.Enabled = False
                End If
                Me.ToolStripButton_Salva.Enabled = False
                Me.ToolStripButton_Annulla.Enabled = False

                Me.DataGridView.Enabled = True

                AbilitaControlli(False)

            Catch ex As Exception
                System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, m_strFormTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End Try
        End If
    End Sub

    Protected Overridable Sub BaseGriglia_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed

    End Sub

    Protected Overrides Sub DateTimePicker_Inizio_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.DesignMode = False Then
            AbilitaControlli(False)
        End If
        MyBase.DateTimePicker_Inizio_ValueChanged(sender, e)
    End Sub

    Protected Overrides Sub DateTimePicker_Fine_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.DesignMode = False Then
            AbilitaControlli(False)
        End If
        MyBase.DateTimePicker_Fine_ValueChanged(sender, e)
    End Sub

    Protected Overrides Sub BaseGriglia_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.DesignMode = False Then
            Static bOneShot As Boolean
            Dim pt As Point

            If bOneShot = False Then
                bOneShot = True
                pt = Me.DataGridView.Location
                Me.DataGridView.Height = Me.DataGridView.Height / 2
                pt.Y = Me.DataGridView.Location.Y + Me.DataGridView.Height
                Me.DataGridView.Location = pt
            End If
        End If
        MyBase.BaseGriglia_SizeChanged(sender, e)
    End Sub

    '' Eventi binding navigator
    'Protected Overridable Sub ToolStripButton_Nuovo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_Nuovo.Click
    '    If Me.DesignMode = False Then
    '        ' Aggiorno
    '        Me.m_bs.Position = -1

    '        Me.ToolStripButton_Nuovo.Enabled = False
    '        Me.ToolStripButton_Elimina.Enabled = False
    '        Me.ToolStripButton_Modifica.Enabled = False
    '        Me.ToolStripButton_Salva.Enabled = True
    '        Me.ToolStripButton_Annulla.Enabled = True

    '        Me.DataGridView.Enabled = False

    '        AbilitaControlli(True)
    '    End If
    'End Sub

    'Protected Overridable Sub ToolStripButton_Elimina_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_Elimina.Click
    '    If Me.DesignMode = False Then
    '        Me.ToolStripButton_Nuovo.Enabled = True
    '        Me.ToolStripButton_Elimina.Enabled = True
    '        Me.ToolStripButton_Modifica.Enabled = True
    '        Me.ToolStripButton_Salva.Enabled = False
    '        Me.ToolStripButton_Annulla.Enabled = False

    '        Me.DataGridView.Enabled = True

    '        AbilitaControlli(False)
    '    End If
    'End Sub

    'Protected Overridable Sub ToolStripButton_Modifica_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_Modifica.Click
    '    If Me.DesignMode = False Then
    '        Me.ToolStripButton_Nuovo.Enabled = False
    '        Me.ToolStripButton_Elimina.Enabled = False
    '        Me.ToolStripButton_Modifica.Enabled = False
    '        Me.ToolStripButton_Salva.Enabled = True
    '        Me.ToolStripButton_Annulla.Enabled = True

    '        Me.DataGridView.Enabled = False

    '        AbilitaControlli(True)
    '    End If
    'End Sub

    'Protected Overridable Sub ToolStripButton_Salva_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_Salva.Click
    '    If Me.DesignMode = False Then
    '        ' Aggiorno
    '        Me.m_bs.Position = -1

    '        Me.ToolStripButton_Nuovo.Enabled = True
    '        Me.ToolStripButton_Elimina.Enabled = True
    '        Me.ToolStripButton_Modifica.Enabled = True
    '        Me.ToolStripButton_Salva.Enabled = False
    '        Me.ToolStripButton_Annulla.Enabled = False

    '        Me.DataGridView.Enabled = True

    '        AbilitaControlli(False)
    '    End If
    'End Sub

    'Protected Overridable Sub ToolStripButton_Annulla_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_Annulla.Click
    '    If Me.DesignMode = False Then
    '        ' Aggiorno
    '        Me.m_bs.Position = -1

    '        Me.ToolStripButton_Nuovo.Enabled = True
    '        Me.ToolStripButton_Elimina.Enabled = True
    '        Me.ToolStripButton_Modifica.Enabled = True
    '        Me.ToolStripButton_Salva.Enabled = False
    '        Me.ToolStripButton_Annulla.Enabled = False

    '        Me.DataGridView.Enabled = True

    '        AbilitaControlli(False)
    '    End If
    'End Sub

    'Protected Overridable Sub AbilitaControlli(ByVal bAbilitaControlli As Boolean)
    '    If Me.DesignMode = False Then

    '    End If
    'End Sub
End Class
