// File: Models/LichHocChiTietDto.cs
using System;

namespace QuanLySinhVien.Models
{
    public class LichHocChiTietDto
    {
        public string TenLopHocPhan { get; set; }
        public string TenGiangVien { get; set; }
        public string TenPH { get; set; }
        public int TietBatDau { get; set; }
        public int TietKetThuc { get; set; }
        
        public string ThoiGian 
        { 
            get 
            {
                return $"{ChuyenTietSangGio(TietBatDau)} - {ChuyenTietSangGio(TietKetThuc)}";
            }
        }

        // -> tiet -> tgian
        private string ChuyenTietSangGio(int tiet)
        {
            return tiet switch
            {
                1 => "07:00",
                2 => "07:50",
                3 => "09:00",
                4 => "09:50",
                5 => "10:40",
                6 => "13:00",
                7 => "13:50",
                8 => "15:00",
                9 => "15:50",
                10 => "16:40",
                _ => "N/A",
            };
        }
    }
}