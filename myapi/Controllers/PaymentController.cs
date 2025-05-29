using Microsoft.AspNetCore.Mvc;
using myapi.Data;
using myapi.Models;
using System.Text;

namespace myapi.Controllers
{
    [Route("apiv1/[controller]/[action]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentRepository _paymentRepository;
        private readonly RoomRepository _roomRepository;

        public PaymentController(PaymentRepository paymentRepository, RoomRepository roomRepository)
        {
            _paymentRepository = paymentRepository;
            _roomRepository = roomRepository;
        }
        
        [HttpGet]
        public IActionResult GetPaymentDetails(int hostelId)
        {
            try
            {
                var payments = _paymentRepository.GetPaymentDetails(hostelId);

                if (!payments.Any())
                {
                    return NotFound(new
                    {
                        Status = "Failure",
                        Message = "No Payment details found"
                    });
                }

                return Ok(new
                {
                    Status = "Success",
                    Data = payments,
                    Message = "Payment details retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = "Failure",
                    Message = "Internal server error",
                    Error = ex.Message
                });
            }
        }

        [HttpPost]   
        public IActionResult PayFee(int roomId, int studentId, int hostelId)
        {
            try
            {
                var room = _roomRepository.GetRoomByID(roomId);
                if (room == null)
                {
                    return NotFound(new
                    {
                        Status = "Failure",
                        Message = "Room not found"
                    });
                }
                
                if (room.RoomRent <= 0)
                {
                    return BadRequest(new
                    {
                        Status = "Failure",
                        Message = "Invalid room rent amount"
                    });
                }

                var paymentModel = new PaymentCreateModel
                {
                    StudentID = studentId,
                    RoomID = roomId,
                    HostelID = hostelId,
                    Amount = room.RoomRent
                };

                int paymentId = _paymentRepository.CreatePayment(paymentModel);

                return Ok(new
                {
                    Status = "Success",
                    Data = new { PaymentID = paymentId, Amount = room.RoomRent },
                    Message = "Payment successful"
                });
            }
            catch (Exception ex)
            {                
                return StatusCode(500, new
                {
                    Status = "Failure",
                    Message = "Error processing payment",
                    Error = ex.Message
                });
            }
        }

        [HttpGet("{studentId}")]
        public IActionResult GetPaymentHistory(int studentId)
        {
            try
            {
                var payments = _paymentRepository.GetPaymentsByStudent(studentId);
                return Ok(new
                {
                    Status = "Success",
                    Data = payments,
                    Message = "Payments retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = "Failure",
                    Message = "Error retrieving payments",
                    Error = ex.Message
                });
            }
        }

        [HttpGet("{paymentId}")]
        public IActionResult DownloadReceipt(int paymentId)
        {
            try
            {
                var payment = _paymentRepository.GetPaymentByID(paymentId);
                if (payment == null)
                {
                    return NotFound(new
                    {
                        Status = "Failure",
                        Message = "Payment not found"
                    });
                }
              
                var invoiceHtml = new StringBuilder();
                invoiceHtml.AppendLine("<!DOCTYPE html>");
                invoiceHtml.AppendLine("<html>");
                invoiceHtml.AppendLine("<head>");
                invoiceHtml.AppendLine("    <meta charset=\"utf-8\" />");
                invoiceHtml.AppendLine($"    <title>Invoice #{payment.PaymentID}</title>");
                invoiceHtml.AppendLine("    <style>");
                invoiceHtml.AppendLine("        body { font-family: Arial, sans-serif; margin: 0; padding: 0; }");
                invoiceHtml.AppendLine("        .invoice-container { margin: 20px auto; padding: 20px; max-width: 800px; border: 1px solid #ddd; }");
                invoiceHtml.AppendLine("        .invoice-header { text-align: center; margin-bottom: 20px; }");
                invoiceHtml.AppendLine("        .invoice-header h1 { margin: 0; }");
                invoiceHtml.AppendLine("        .invoice-from-to { margin-bottom: 20px; }");
                invoiceHtml.AppendLine("        .invoice-from-to .from, .invoice-from-to .to { width: 45%; display: inline-block; vertical-align: top; }");
                invoiceHtml.AppendLine("        .invoice-from-to .from { text-align: left; }");
                invoiceHtml.AppendLine("        .invoice-from-to .to { text-align: right; }");
                invoiceHtml.AppendLine("        .invoice-items table { width: 100%; border-collapse: collapse; }");
                invoiceHtml.AppendLine("        .invoice-items th, .invoice-items td { padding: 8px; border: 1px solid #ddd; text-align: left; }");
                invoiceHtml.AppendLine("        .invoice-footer { text-align: right; margin-top: 20px; }");
                invoiceHtml.AppendLine("    </style>");
                invoiceHtml.AppendLine("</head>");
                invoiceHtml.AppendLine("<body>");
                invoiceHtml.AppendLine("    <div class=\"invoice-container\">");
                invoiceHtml.AppendLine("        <div class=\"invoice-header\">");
                invoiceHtml.AppendLine("            <h1>Invoice</h1>");
                invoiceHtml.AppendLine($"            <p><strong>Date:</strong> {payment.PaymentDate.ToString("dd MMM yyyy")}</p>");
                invoiceHtml.AppendLine($"            <p><strong>Status:</strong> {payment.PaymentStatus}</p>");
                invoiceHtml.AppendLine("        </div>");
                invoiceHtml.AppendLine("        <div class=\"invoice-from-to\">");
                invoiceHtml.AppendLine("            <div class=\"from\">");
                invoiceHtml.AppendLine("                <h4>From:</h4>");
                invoiceHtml.AppendLine($"                <p><strong>{payment.HostelName}</strong></p>");                                
                invoiceHtml.AppendLine($"                <p>Email: {payment.HostelEmail}</p>");
                invoiceHtml.AppendLine($"                <p>Phone: +91 {payment.HostelContactNumber}</p>");
                invoiceHtml.AppendLine("            </div>");
                invoiceHtml.AppendLine("            <div class=\"to\">");
                invoiceHtml.AppendLine("                <h4>To:</h4>");
                invoiceHtml.AppendLine($"                <p><strong>{payment.StudentName}</strong></p>");
                invoiceHtml.AppendLine($"                <p>Email: {payment.StudentEmail}</p>");
                invoiceHtml.AppendLine($"                <p>Phone: +91 {payment.StudentPhoneNumber}</p>");
                invoiceHtml.AppendLine("            </div>");
                invoiceHtml.AppendLine("            <div style=\"clear: both;\"></div>");
                invoiceHtml.AppendLine("        </div>");
                invoiceHtml.AppendLine("        <div class=\"invoice-items\">");
                invoiceHtml.AppendLine("            <table>");
                invoiceHtml.AppendLine("                <thead>");
                invoiceHtml.AppendLine("                    <tr>");
                invoiceHtml.AppendLine("                        <th>#</th>");
                invoiceHtml.AppendLine("                        <th>Item</th>");
                invoiceHtml.AppendLine("                        <th>Description</th>");                                
                invoiceHtml.AppendLine("                        <th>Total</th>");
                invoiceHtml.AppendLine("                    </tr>");
                invoiceHtml.AppendLine("                </thead>");
                invoiceHtml.AppendLine("                <tbody>");
                invoiceHtml.AppendLine("                    <tr>");
                invoiceHtml.AppendLine("                        <td>1</td>");
                invoiceHtml.AppendLine("                        <td>Room Rent</td>");
                invoiceHtml.AppendLine("                        <td>Monthly Room Rent Payment</td>");                               
                invoiceHtml.AppendLine($"                        <td>₹{payment.Amount}</td>");
                invoiceHtml.AppendLine("                    </tr>");
                invoiceHtml.AppendLine("                </tbody>");
                invoiceHtml.AppendLine("            </table>");
                invoiceHtml.AppendLine("        </div>");
                invoiceHtml.AppendLine("        <div class=\"invoice-footer\">");
                invoiceHtml.AppendLine($"            <h3>Total: ₹{payment.Amount}</h3>");
                invoiceHtml.AppendLine("        </div>");
                invoiceHtml.AppendLine("    </div>");
                invoiceHtml.AppendLine("</body>");
                invoiceHtml.AppendLine("</html>");
               
                byte[] invoiceBytes = Encoding.UTF8.GetBytes(invoiceHtml.ToString());
                return File(invoiceBytes, "text/html", $"Invoice_{payment.PaymentID}.html");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = "Failure",
                    Message = "Error generating invoice",
                    Error = ex.Message
                });
            }
        }
    }
}
