Public Class StatoElab
    Public ReadOnly Property Annulla() As String
        Get
            Annulla = m_bAnnulla
            Return Annulla
        End Get
    End Property

    Private m_bAnnulla As Boolean

    Private Sub StatoElab_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_bAnnulla = False
        Me.ProgressBar_1.Value = 1
    End Sub

    Public Sub NewStep()
        If Me.ProgressBar_1.Value >= 100 Then
            Me.ProgressBar_1.Value = 1
        End If
        Me.ProgressBar_1.Value = Me.ProgressBar_1.Value + 1
    End Sub

    Private Sub Button_Annulla_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Annulla.Click
        m_bAnnulla = True
    End Sub
End Class