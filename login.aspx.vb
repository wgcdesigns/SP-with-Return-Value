
    Protected Sub btnLogin_Click(sender As Object, e As System.EventArgs) Handles btnLogin.Click

        ValidateUser()

    End Sub

    Protected Sub ValidateUser()
        Dim userId As Integer = 0
        pnlError.Visible = False
        If String.IsNullOrEmpty(txtUserName.Text.Trim) Then

            lblMessage.Text = "A Username is required."
            pnlError.Visible = True
            txtUserName.Focus()
            Exit Sub

        ElseIf String.IsNullOrEmpty(txtPassword.Text.Trim) Then

            lblMessage.Text = "A Password is required."
            pnlError.Visible = True
            txtPassword.Focus()
            Exit Sub

        End If

        Dim SPResponse As Integer = Nothing
        Dim constr As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand("dbo.sp_UserLogin")
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@pLoginName", txtUserName.Text.Trim)
                cmd.Parameters.AddWithValue("@pPassword", txtPassword.Text.Trim)

                ' ADD RESPONSE VALUE TO PARAMETERS SO WE GET ONE BACK =================
                cmd.Parameters.Add("@responseMessage", SqlDbType.Int)
                cmd.Parameters("@responseMessage").Direction = ParameterDirection.Output
                ' ======================================================================

                cmd.Connection = con
                con.Open()

                cmd.ExecuteNonQuery()
                SPResponse = CInt(cmd.Parameters("@responseMessage").Value)
                con.Close()

            End Using
            Select Case SPResponse
                Case 0
                    'Login1.FailureText = "Username and/or password is incorrect."
                    lblTestStatus.Text = "Invalid login"
                    Exit Select
                Case Is >= 1
                    lblTestStatus.Text = "User successfully logged in"

            End Select

        End Using
    End Sub