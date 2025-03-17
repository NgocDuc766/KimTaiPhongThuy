using KimTaiPhongThuy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace KimTaiPhongThuy.Pages.Chat
{
    public class IndexModel : PageModel
    {
        private JewelryStoreContext _context;
        public IndexModel(JewelryStoreContext context)
        {
            _context = context;
        }

        private int UserId { get; set; }


        public void OnGet()
        {
        }

        [BindProperty]
        public ChatHistory NewChat { get; set; }

        // Xử lý lưu tin nhắn
        public async Task<IActionResult> OnPostSaveChatAsync()
        {
            try
            {
                using (var reader = new System.IO.StreamReader(Request.Body))
                {
                    var body = await reader.ReadToEndAsync();
                    Console.WriteLine($"Request body: {body}"); // Log để kiểm tra dữ liệu nhận được

                    var chatData = System.Text.Json.JsonSerializer.Deserialize<ChatHistory>(body);

                    if (chatData == null || string.IsNullOrEmpty(chatData.Message))
                    {
                        Console.WriteLine("Dữ liệu không hợp lệ: Thiếu message hoặc sender");
                        return BadRequest("Dữ liệu không hợp lệ: Thiếu message hoặc sender");
                    }

                    Console.WriteLine($"Chat Data - Message: {chatData.Message}, Sender: {chatData.Sender}"); // Log dữ liệu sau deserialize

                    chatData.UserId = 1; // Thay bằng UserId thực tế
                    chatData.Timestamp = DateTime.Now;

                    Console.WriteLine("Thêm chatData vào context...");
                    _context.ChatHistories.Add(chatData);

                    Console.WriteLine("Lưu vào database...");
                    int rowsAffected = await _context.SaveChangesAsync();
                    Console.WriteLine($"Đã lưu thành công, số dòng ảnh hưởng: {rowsAffected}");

                    return new JsonResult(new { success = true });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi server khi lưu chat: {ex.Message}\nStackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Lỗi server khi lưu chat: {ex.Message}");
            }
        }

        // Xử lý lấy lịch sử chat
        public IActionResult OnGetChatHistory()
        {
            int userId = UserId; // Thay bằng UserId thực tế

            var history = _context.ChatHistories
                .Where(ch => ch.UserId == userId)
                .OrderBy(ch => ch.Timestamp)
                .Select(ch => new
                {
                    ch.Message,
                    ch.Sender,
                    ch.Timestamp
                })
                .ToList();
      
                return new JsonResult(history);
            
        }
    }
}
