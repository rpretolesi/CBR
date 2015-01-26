'Imports System.Data
Imports System.Data.SqlClient

Public Class Login

    Private m_ld As New LoginData
    Private m_sdiFormIcon As Icon
    Private m_strFormTitle As String
    Private m_strConnStringParamName As String

    Public ReadOnly Property GetLoginData() As LoginData
        Get
            Return Me.m_ld
        End Get
    End Property

    Private Sub Login_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = m_strFormTitle
        Me.Icon = m_sdiFormIcon
    End Sub

    Overloads Function ShowDialog(ByVal iLevel As Integer, ByVal ldLoginData As LoginData, ByVal sdiFormIcon As Icon, ByVal strFormTitle As String, ByVal strConnStringParamName As String) As Windows.Forms.DialogResult

        Me.TextBox_Utente.Text = ""
        Me.TextBox_Password.Text = ""
        Me.TextBox_Level_Required.Text = iLevel.ToString

        Me.m_ld.Value = ldLoginData
        Me.m_sdiFormIcon = sdiFormIcon
        Me.m_strFormTitle = strFormTitle
        Me.m_strConnStringParamName = strConnStringParamName

        If Me.m_ld.lLivello >= iLevel Then
            Return Windows.Forms.DialogResult.Yes
        End If

        Return MyBase.ShowDialog()

    End Function

    Private Sub Button_Login_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Login.Click
        ' Aggiorno i dati immessi
        Me.Validate()

        Dim bRes As Boolean

        Dim strSQL As String
        Dim ds As New DataSet()

        Try
            strSQL = "SELECT * FROM Utenti WHERE U_Utente=" + "'" + Me.TextBox_Utente.Text + "'" + " AND " + "U_Password=" + "'" + Me.TextBox_Password.Text + "'" + " AND " + "U_Livello>=" + Me.TextBox_Level_Required.Text
            ApriDataSet(strSQL, "", ds, Me.m_ld, m_strFormTitle, m_strConnStringParamName)
            Try
                ' Verifico Username, Password e Livello
                If ds.Tables(0).Rows.Count > 0 Then
                    ' L'unicita' e' definita nel database
                    For Each row As DataRow In ds.Tables(0).Rows
                        Me.m_ld.iID_Utente = row.Item("U_ID")
                        Me.m_ld.strU_Utente = row.Item("U_Utente").ToString
                        Me.m_ld.strU_Password = row.Item("U_Password").ToString
                        Me.m_ld.lLivello = row.Item("U_Livello")
                        Me.m_ld.strNome = row.Item("U_Nome").ToString
                        Me.m_ld.strCognome = row.Item("U_Cognome").ToString
                    Next row

                    bRes = True

                    AddLogEvent(LOG_OK, "Login Ok.", Me.m_ld, m_strFormTitle, m_strConnStringParamName)
                    System.Windows.Forms.MessageBox.Show("Login Ok.", m_strFormTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)

                Else
                    bRes = False

                    AddLogEvent(LOG_ERROR, "User o Password non corretti oppure Livello insufficiente.", Me.m_ld, m_strFormTitle, m_strConnStringParamName)
                    System.Windows.Forms.MessageBox.Show("User o Password non corretti oppure Livello insufficiente.", m_strFormTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop)

                End If

            Catch ex As Exception

                bRes = False

                AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Me.m_ld, m_strFormTitle, m_strConnStringParamName)
                System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, m_strFormTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop)

            End Try

        Catch ex As SqlException

            bRes = False

            AddLogEvent(LOG_ERROR, ex.Message + ex.StackTrace, Me.m_ld, m_strFormTitle, m_strConnStringParamName)
            System.Windows.Forms.MessageBox.Show(ex.Message + ex.StackTrace, m_strFormTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop)

        End Try

        ' Chiudo
        If bRes = True Then
            Me.DialogResult = Windows.Forms.DialogResult.Yes
        Else
            Me.DialogResult = Windows.Forms.DialogResult.No
        End If

    End Sub

    Private Sub Button_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Cancel.Click

        Me.DialogResult = Windows.Forms.DialogResult.Cancel

    End Sub

    Public Sub Logout(ByVal ldLoginData As LoginData)

        Me.m_ld.ResetValue()

        AddLogEvent(LOG_OK, "Logout Ok.", Me.m_ld, m_strFormTitle, m_strConnStringParamName)
        System.Windows.Forms.MessageBox.Show("Logout Ok.", m_strFormTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

End Class
