Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Windows.Forms


Public Class Search
    Inherits System.Windows.Forms.Form
    Dim strSQLMem As String
    Dim dtMem As New DataTable()
    Dim intTotalRows As Integer
    Dim intCurrentRow As Integer
    Dim bdSource As New BindingSource
    Dim intID As Integer
    Dim massageTay As Integer
    Dim ds As DataSet
    Dim DateFrom As DateTime
    Dim DateTo As DateTime
    Dim cnn As New OleDb.OleDbConnection
    Private Sub DateTimePicker1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateTimePicker1.ValueChanged
        txtFrom.Text = DateTimePicker1.Value.ToShortDateString()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Me.Visible = False
        FrmSignIn.Show()
    End Sub

    Private Sub btnTimeInSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTimeInSubmit.Click
        If txtFrom.Text <> "" Or txtTo.Text <> "" Then
            DateFrom = Convert.ToDateTime(txtFrom.Text)
            DateTo = Convert.ToDateTime(txtTo.Text)


            cnn = New OleDb.OleDbConnection
            cnn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source = " & Application.StartupPath & "\PodSignIn2.mdb"
            If Not cnn.State = ConnectionState.Open Then
                cnn.Open()
            End If
            ' DateAndTime.Now.AddHours(2)
            'Dim da As New OleDb.OleDbDataAdapter("SELECT S.ID, S.SignInName, C.ComputerNumber, S.TimeIn, S.TimeOut FROM SignIn2 S LEFT JOIN ComputerList C ON C.ID = S.StationNumber WHERE Status = 1", cnn)
            Dim da As New OleDb.OleDbDataAdapter("SELECT S.ID, S.SignInName, S.TimeIn, S.TimeOut, C.ComputerNumber" & _
                                                 " FROM SignIn2 S LEFT JOIN ComputerList C ON C.ID = S.HistoryStationNumber WHERE S.InsertDate >= #" & _
                DateFrom & "# AND S.InsertDate <= #" & DateTo.AddDays(1) & "# ORDER BY S.TimeIn", cnn)
            Dim dt As New DataTable
            da.Fill(dt)
            cnn.Close()
            DataGridViewSearch.DataSource = dt
            Me.DataGridViewSearch.Columns(0).Visible = False
            Me.DataGridViewSearch.Columns(2).Visible = False
            'Me.DataGridViewSearch.Columns(4).Visible = False
            DataGridViewSearch.Visible = True
            Me.Visible = True


        Else
            MessageBox.Show(" Please input fields. ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
        'FrmSignIn.Show()
        'Me.Visible = False

    End Sub

    Private Sub Panel1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub DateTimePicker2_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateTimePicker2.ValueChanged
        txtTo.Text = DateTimePicker2.Value.ToShortDateString()
    End Sub

    Private Sub Search_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        DateTimePicker1.Value = Date.Now()
        DateTimePicker2.Value = Date.Now()
    End Sub
End Class