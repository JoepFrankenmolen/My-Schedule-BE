using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace My_Schedule.NotificationService.Migrations
{
    /// <inheritdoc />
    public partial class v13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Group
            migrationBuilder.Sql(@"
                INSERT INTO [dbo].[NotificationGroups]
                ([Id], [Name])
                VALUES
                ('fa66e1fd-dbda-43f5-a2ed-008d40d12d19', 'Auth Group');
            ");

            // Notification
            // 1
            migrationBuilder.Sql(@"
                INSERT INTO [dbo].[Notifications]
                ([Id], [Type], [Title], [Description], [GroupId], [EmailPreference], [IsEnforced])
                VALUES
                ('8605b62e-975c-475c-ab6a-f4c32e97b8b8', 1, 'EmailConfirmation', 'Confirm the email address', 'fa66e1fd-dbda-43f5-a2ed-008d40d12d19', 1, 1);
            ");

            // 2
            migrationBuilder.Sql(@"
                INSERT INTO [dbo].[Notifications]
                ([Id], [Type], [Title], [Description], [GroupId], [EmailPreference], [IsEnforced])
                VALUES
                ('76af0a97-63c4-4306-af3b-f26d46b51761', 2, 'LoginVerification', 'verifiy if the user logging in has access to the email.', 'fa66e1fd-dbda-43f5-a2ed-008d40d12d19', 1, 1);
            ");

            // 3
            migrationBuilder.Sql(@"
                INSERT INTO [dbo].[Notifications]
                ([Id], [Type], [Title], [Description], [GroupId], [EmailPreference], [IsEnforced])
                VALUES
                ('f7b162ad-8548-43b9-be66-5c029243f501', 3, 'PasswordReset', 'Reset the password.', 'fa66e1fd-dbda-43f5-a2ed-008d40d12d19', 1, 1);
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Group
            migrationBuilder.Sql(@"
                DELETE FROM [dbo].[NotificationGroups]
                WHERE Id = 'fa66e1fd-dbda-43f5-a2ed-008d40d12d19';
            ");

            // Notification
            // Group
            migrationBuilder.Sql(@"
                DELETE FROM [dbo].[Notifications]
                WHERE Id = 'f7b162ad-8548-43b9-be66-5c029243f501' or Id = '76af0a97-63c4-4306-af3b-f26d46b51761' or Id = '8605b62e-975c-475c-ab6a-f4c32e97b8b8';
            ");
        }
    }
}
