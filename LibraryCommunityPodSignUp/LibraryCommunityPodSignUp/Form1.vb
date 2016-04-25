
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Windows.Forms


Public Class FrmSignIn
    Inherits Windows.Forms.Form
    Dim strSQLMem As String
    Dim dtMem As New DataTable()
    Dim intCurrentRow As Integer
    Dim bdSource As New BindingSource
    Dim ds As DataSet
    Dim strSongToPlay As String
    Dim intRowToPlay As Integer
    Dim intID As Integer
    Dim SignInTime As Date
    Dim intSignInHr As Int32
    Public Const EmpNumMAX As Integer = 15
    Dim cnn As New OleDb.OleDbConnection
    Private Sub btnTimeInSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTimeInSubmit.Click
        Dim cmd As New OleDb.OleDbCommand
        cnn = New OleDb.OleDbConnection
        cnn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source = " & Application.StartupPath & "\PodSignIn2.mdb"
        If Not cnn.State = ConnectionState.Open Then
            cnn.Open()
        End If
        cmd.Connection = cnn
        
        If txtName.Text = "" Then ' Or cbComputer.SelectedValue = 0 Then
            MessageBox.Show("Please enter name and computer number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Else
            SignInTime = TimeValue(Now)

            cmd.CommandText = "INSERT INTO SignIn2(SignInName, TimeIn, TimeOut, StationNumber, InsertDate, Status, HistoryStationNumber)" & _
                   "VALUES(" & """" & txtName.Text & """, """ & DateAndTime.Now & """, """ & DateAndTime.Now.AddHours(2) & """, """ & Convert.ToInt32(cbComputer.SelectedValue.ToString()) & """, """ & Date.Now() & """, """ & 1 & """, """ & Convert.ToInt32(cbComputer.SelectedValue.ToString()) & """)"
            cmd.ExecuteNonQuery()
            cnn.Close()
            OpenSignInListDB()
            txtName.Text = ""

        End If
    End Sub
    Private Sub DataGridViewSignUp_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridViewSignUp.CellContentClick
        With Me.DataGridViewSignUp.Rows(e.RowIndex)
            Dim count As Int16
            count = 30
            Select Case e.ColumnIndex
                Case 0
                    intID = .Cells("ID").Value
                    Dim cmd As New OleDb.OleDbCommand
                    If Not cnn.State = ConnectionState.Open Then
                        cnn.Open()
                    End If
                    cmd.Connection = cnn
                    cmd.CommandText = "UPDATE SignIn2 SET Status = 0, StationNumber = NULL, TimeOut = """ & DateAndTime.Now & """ WHERE ID =" & intID
                    cmd.ExecuteNonQuery()
                    cnn.Close()
                    OpenSignInListDB()
                    
                    'Case 1
                    '    intID = .Cells("ID").Value
                    '    txtFname.Text = .Cells("FirstName").Value
                    '    txtLName.Text = .Cells("LastName").Value
                    '    txtPhone.Text = .Cells("Phone").Value
                    '    Me.cbEmpNum.SelectedIndex = .Cells("EmpNum").Value - 1
                    '    'If .Cells("CashOnly").Value = 0 Then
                    '    '    ChkCashOnly.Checked = False
                    '    'Else
                    '    '    ChkCashOnly.Checked = True
                    '    'End If
                    '    PnlEdit.Visible = True
                    '    btnUpdate.Visible = True
                    '    btnAdd.Visible = False
                    '    lblEditPanelHeader.Text = "Please Fill Employee data and click Update"
            End Select

        End With
    End Sub
    Private Sub FrmSignIn_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            OpenSignInListDB()
        Catch ex As Exception
            MessageBox.Show("Cannot connect", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub
    Public Sub OpenSignInListDB()
        cnn = New OleDb.OleDbConnection
        'cnn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source = " & Application.StartupPath & "\JoAnn.mdb"
        cnn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source = " & Application.StartupPath & "\PodSignIn2.mdb"
        If Not cnn.State = ConnectionState.Open Then
            cnn.Open()
        End If

        Dim da As New OleDb.OleDbDataAdapter("SELECT S.ID, S.SignInName, C.ComputerNumber, S.TimeIn, S.TimeOut FROM SignIn2 S LEFT JOIN ComputerList C ON C.ID = S.StationNumber WHERE Status = 1", cnn)
        'Dim da As New OleDb.OleDbDataAdapter("SELECT SignInName FROM SignIn1 ", cnn)
        Dim dt As New DataTable
        da.Fill(dt)
        cnn.Close()
        Me.DataGridViewSignUp.DataSource = dt

      

        Dim da2 As New OleDb.OleDbDataAdapter("SELECT C.ID,  C.ComputerNumber FROM ComputerList C LEFT JOIN SignIn2 S ON C.ID = S.StationNumber  WHERE S.StationNumber IS NULL ORDER BY ComputerNumber", cnn)
        Dim dt2 As New DataTable
        da2.Fill(dt2)
        cnn.Close()
        Me.cbComputer.DataSource = dt2
        cbComputer.DisplayMember = "ComputerNumber"
        cbComputer.ValueMember = "ID"
        Me.cbComputer.Refresh()
        SetDataGridViewBackColor()
        Me.DataGridViewSignUp.Columns(1).Visible = False
        Me.DataGridViewSignUp.Columns(2).Visible = False
        Me.DataGridViewSignUp.Refresh()
    End Sub

    Public Sub SetDataGridViewBackColor()
       

        For i = 0 To DataGridViewSignUp.Rows.Count - 1
            Dim timeOut As DateTime = DateTime.Parse(DataGridViewSignUp.Rows(i).Cells("TimeOut").Value)

            
            If timeOut.TimeOfDay < DateTime.Now.TimeOfDay Then
                DataGridViewSignUp.Rows(i).DefaultCellStyle.BackColor = Color.Red
            End If

        Next
    End Sub

   
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Search.Show()
        Me.Visible = False
    End Sub
End Class
