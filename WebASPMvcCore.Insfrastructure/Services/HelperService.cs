using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using WebASPMvcCore.Insfrastructure.Abstracts;

namespace WebASPMvcCore.Insfrastructure.Services
{
    public class HelperService: IHelperService
    {
        public string ConvertToSlug(string str)
        {
            string[] VietNamChar = new string[]{
                "aAeEoOuUiIdDyY",
                "áàạảãâấầậẩẫăắằặẳẵ",
                "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
                "éèẹẻẽêếềệểễ",
                "ÉÈẸẺẼÊẾỀỆỂỄ",
                "óòọỏõôốồộổỗơớờợởỡ",
                "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
                "úùụủũưứừựửữ",
                "ÚÙỤỦŨƯỨỪỰỬỮ",
                "íìịỉĩ",
                "ÍÌỊỈĨ",
                "đ",
                "Đ",
                "ýỳỵỷỹ",
                "ÝỲỴỶỸ"
            };
            //Thay thế và lọc dấu từng char      
            for (int i = 1; i < VietNamChar.Length; i++)
            {
                for (int j = 0; j < VietNamChar[i].Length; j++)
                    str = str.Replace(VietNamChar[i][j], VietNamChar[0][i - 1]);
            }
            str = str.ToLower();
            // Thay dấu cách bằng dấu gạch ngang
            str = str.Replace(" ", "-");
            str = str.Replace(")", "");
            str = str.Replace("(", "");
            return str;
        }
        
    }
}
