using System;
using System.Net;
using System.Net.Mail;

namespace LaptopStore.Service

{
	public class EmailUtil
	{
		public static void SendEmail(string recipientEmail, string subject, string messageBody)
		{
			string host = "smtp.gmail.com"; // SMTP của Gmail
			string from = "22130108@st.hcmuaf.edu.vn"; // Email của bạn
			string password = "gvrd ebra yxuq afiy"; // Mật khẩu ứng dụng hoặc mã xác thực

			try
			{
				// Tạo một đối tượng SmtpClient để kết nối với máy chủ SMTP của Gmail
				using (SmtpClient smtpClient = new SmtpClient(host, 587))
				{
					// Cấu hình kết nối SMTP
					smtpClient.Credentials = new NetworkCredential(from, password);
					smtpClient.EnableSsl = true;

					// Tạo nội dung email với HTML
					string htmlContent = $"<h2>Mã xác nhận của bạn là: <span style='color: red; font-size: 30px'>{messageBody}</span></h2>";

					// Tạo đối tượng MailMessage
					using (MailMessage message = new MailMessage(from, recipientEmail))
					{
						message.Subject = subject;
						message.Body = htmlContent;
						message.IsBodyHtml = true; // Đảm bảo email có định dạng HTML

						// Gửi email
						smtpClient.Send(message);
					}
				}

				Console.WriteLine("Email đã được gửi thành công.");
			}
			catch (Exception ex)
			{
				Console.WriteLine("Lỗi khi gửi email: " + ex.Message);
			}
		}
	}
}
