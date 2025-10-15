DROP DATABASE `QuanLySinhVien`;
CREATE DATABASE `QuanLySinhVien`;
USE `QuanLySinhVien`;

-- 1
CREATE TABLE `SinhVien` (
                            `MaSV` INT AUTO_INCREMENT PRIMARY KEY,
                            `MaTK` INT,
                            `MaLop` INT,
                            `MaKhoaHoc` INT,
                            `TenSV` VARCHAR(255),
                            `SoDienThoaiSV` VARCHAR(255),
                            `NgaySinhSV` VARCHAR(255),
                            `QueQuanSV` VARCHAR(255),
                            `TrangThaiSV` VARCHAR(255),
                            `GioiTinhSV` VARCHAR(255),
                            `EmailSV` VARCHAR(255),
                            `CCCDSV` VARCHAR(255),
                            `AnhDaiDienSV` VARCHAR(255),
                            Status TINYINT DEFAULT 1
);

-- 2
CREATE TABLE Lop     (
                         MaLop   INT AUTO_INCREMENT PRIMARY KEY ,
                         MaGV INT,
                         MaNganh  INT,
                         TenLop   VARCHAR(255),
                         SoLuongSV INT,
                         Status TINYINT DEFAULT 1
);

-- 3
CREATE TABLE GiangVien   (
                             MaGV INT AUTO_INCREMENT PRIMARY KEY ,
                             MaTK INT,
                             MaKhoa INT,
                             TenGV  VARCHAR(255),
                             NgaySinhGV DATE,
                             GioiTinhGV VARCHAR(255),
                             SoDienThoai VARCHAR(255),
                             Email  VARCHAR(255),
                             TrangThai VARCHAR(255),
                             AnhDaiDienGV VARCHAR(255),
                             Status TINYINT DEFAULT 1
);

-- 4
CREATE TABLE Khoa    (
                         MaKhoa  INT AUTO_INCREMENT PRIMARY KEY ,
                         TenKHoa  VARCHAR(255),
                         Email  VARCHAR(255),
                         DiaChi  VARCHAR(255),
                         Status TINYINT DEFAULT 1
);

-- 5
CREATE TABLE Nganh      (
                            MaNganh    INT AUTO_INCREMENT PRIMARY KEY ,
                            MaKhoa  INT,
                            TenNganh   VARCHAR(255),
                            Status TINYINT DEFAULT 1
);

-- 6
CREATE TABLE ChuyenNganh      (
                                  MaCN     INT AUTO_INCREMENT PRIMARY KEY ,
                                  MaNganh   INT,
                                  TenCN    VARCHAR(255),
                                  Status TINYINT DEFAULT 1
);

-- 7
create table HocPhiTinChi(
                             MaHPTC INT AUTO_INCREMENT PRIMARY KEY,
                             MaNganh INT, -- FK

                             SoTienMotTinChi double,
                             HocKy INT,
                             Nam VARCHAR(255),
                             Status TINYINT DEFAULT 1
);


-- 8
create table HocPhiSV(
                         MaHP INT AUTO_INCREMENT PRIMARY KEY,
                         MaSV INT, -- FK

                         HocKy VARCHAR(255),
                         Nam VARCHAR(255),
                         TongHocPhi double,
                         DaThu double,
                         TrangThai VARCHAR(255),
                         Status TINYINT DEFAULT 1
);

-- 9
create table KhoaHoc(
                        MaKhoaHoc INT AUTO_INCREMENT PRIMARY KEY,
                        MaCKDT INT, -- FK

                        TenKhoaHoc VARCHAR(255),
                        NienKhoaHoc VARCHAR(255),
                        Status TINYINT DEFAULT 1
);

-- 10
create table ChuKyDaoTao(
                            MaCKDT INT AUTO_INCREMENT PRIMARY KEY,

                            NamBatDau VARCHAR(255),
                            NamKetThuc VARCHAR(255),
                            Status TINYINT DEFAULT 1
);

-- 11
CREATE TABLE `DangKy` (
                          `MaNHP` INT,
                          `MaSV` INT,
                          PRIMARY KEY (`MaNHP`, `MaSV`),
                          Status TINYINT DEFAULT 1
);

-- 12
CREATE TABLE `NhomHocPhan` (
                               `MaNHP` INT AUTO_INCREMENT PRIMARY KEY,
                               `MaGV` INT,
                               `MaHocPhan` INT,
                               `HocKy` INT,
                               `Nam` VARCHAR(255),
                               `SiSo` INT,
                               Status TINYINT DEFAULT 1
);

-- 13
CREATE TABLE `LichHoc` (
                           `MaLH` INT AUTO_INCREMENT PRIMARY KEY,
                           `MaPH` INT,
                           `MaNHP` INT,
                           `Thu` INT,
                           `TietBatDau` INT,
                           `TuNgay` date,
                           `DenNgay` date,
                           `TietKetThuc` INT,
                           `SoTiet` INT,
                           Status TINYINT DEFAULT 1
);

-- 14
CREATE TABLE `PhongHoc` (
                            `MaPH` INT AUTO_INCREMENT PRIMARY KEY,
                            `TenPH` VARCHAR(255),
                            `LoaiPH` VARCHAR(255),
                            `CoSo` VARCHAR(255),
                            `SucChua` INT,
                            `TinhTrang` VARCHAR(255),
                            Status TINYINT DEFAULT 1
);

-- 15
CREATE TABLE `HocPhan` (
                           `MaHP` INT AUTO_INCREMENT PRIMARY KEY,
                           `MaHPTruoc` INT,
                           `TenHP` VARCHAR(255),
                           `SoTinChi` INT,
                           `HeSoHocPhan` float,
                           `SoTietLyThuyet` INT,
                           `SoTietThucHanh` INT,
                           Status TINYINT DEFAULT 1
);

-- 16
CREATE TABLE `ChiTietDiem` (
                               `MaCTD` INT AUTO_INCREMENT PRIMARY KEY,
                               `MaSV` INT,
                               `MaHP` INT,
                               `HocKy` INT,
                               `Nam` VARCHAR(255),
                               Status TINYINT DEFAULT 1
);

-- 17
CREATE TABLE `KetQua` (
                          `MaKQ` INT AUTO_INCREMENT PRIMARY KEY,
                          `MaCTD` INT,
                          `DiemHe4` float,
                          `DiemHe10` float,
                          Status TINYINT DEFAULT 1
);

-- 18
CREATE TABLE `DiemThi` (
                           `MaDT` INT AUTO_INCREMENT PRIMARY KEY,
                           `MaCTD` INT,
                           `DiemSo` float,
                           Status TINYINT DEFAULT 1
);

-- 19
CREATE TABLE `DiemQuaTrinh` (
                                `MaDQT` INT AUTO_INCREMENT PRIMARY KEY,
                                `MaCTD` INT,
                                `DiemSo` float,
                                Status TINYINT DEFAULT 1
);

-- 20
CREATE TABLE `CotDiem` (
                           `MaCD` INT AUTO_INCREMENT PRIMARY KEY,
                           `MaDQT` INT,
                           `MaHP` INT,
                           `TenCotDiem` VARCHAR(255),
                           `DiemSo` float,
                           `HeSo` float,
                           Status TINYINT DEFAULT 1
);

-- 21
CREATE TABLE `HocPhiHocPhan` (
                                 `MaSV` INT,
                                 `MaHP` INT,
                                 `TongTien` double,
                                 PRIMARY KEY (`MaSV`, `MaHP`),
                                 Status TINYINT DEFAULT 1
);

-- 22
CREATE TABLE `CaThi` (
                         `MaCT` INT AUTO_INCREMENT PRIMARY KEY,
                         `MaHP` INT,
                         `MaPH` INT,
                         `Thu` INT,
                         `ThoiGianBatDau` VARCHAR(255),
                         `ThoiLuong` INT,
                         Status TINYINT DEFAULT 1
);

-- 23
CREATE TABLE `CaThi_SinhVien` (
                                  `MaCT` INT,
                                  `MaSV` INT,
                                  PRIMARY KEY (`MaCT`, `MaSV`),
                                  Status TINYINT DEFAULT 1
);

-- 24
CREATE TABLE ChuongTrinhDaoTao_HocPhan(
                                          MaCTDT INT,
                                          MaHP INT,
                                          PRIMARY KEY (MaCTDT, MaHP),
                                          Status TINYINT DEFAULT 1
);

-- 25
create table ChuongTrinhDaoTao(
                                  MaCTDT INT AUTO_INCREMENT PRIMARY KEY,
                                  MaNganh INT, -- FK

                                  TenCTDT VARCHAR(255),
                                  LoaiHinhDT VARCHAR(255),
                                  TrinhDo VARCHAR(255),
                                  Status TINYINT DEFAULT 1
);

-- 26
CREATE TABLE TaiKhoan (
                          MaTK INT AUTO_INCREMENT PRIMARY KEY ,
                          MaNQ INT ,
                          TenDangNhap VARCHAR(255),
                          MatKhau VARCHAR(255),
                          Status TINYINT DEFAULT 1
);

-- 27
CREATE TABLE NhomQuyen  (
                            MaNQ INT AUTO_INCREMENT  PRIMARY KEY ,
                            TenNhomQuyen VARCHAR(255),
                            Status TINYINT DEFAULT 1
);

-- 28
CREATE TABLE ChiTietQuyen (
                              MaCN INT,
                              MaNQ INT ,
                              HanhDong VARCHAR(255),
                              PRIMARY KEY(MaCN, MaNQ, HanhDong),
                              Status TINYINT DEFAULT 1
);

-- 29
CREATE TABLE ChucNang  (
                           MaCN INT AUTO_INCREMENT PRIMARY KEY ,
                           TenChucNang VARCHAR(255),
                           Status TINYINT DEFAULT 1
);

-- ------------------------INSERT-----------------------------------
-- 1
INSERT INTO `SinhVien` (MaTK, MaLop, MaKhoaHoc, TenSV, SoDienThoaiSV, NgaySinhSV, QueQuanSV, TrangThaiSV, GioiTinhSV, EmailSV, CCCDSV, AnhDaiDienSV) VALUES
                                                                                                                                                         (1, 1, 2, 'Nguyen Van A', '0901111222', '2004-01-15', 'Hà Nội', 'Đang học', 'Nam', 'vana@gmail.com', '123456789', 'a.jpg'),
                                                                                                                                                         (1, 1, 2, 'Tran Thi B', '0902222333', '2004-05-22', 'Đà Nẵng', 'Đang học', 'Nữ', 'thib@gmail.com', '987654321', 'b.jpg'),
                                                                                                                                                         (1, 2, 3, 'Le Van C', '0903333444', '2005-03-10', 'HCM', 'Đang học', 'Nam', 'vanc@gmail.com', '456789123', 'c.jpg'),
                                                                                                                                                         (1, 2, 3, 'Pham Thi D', '0904444555', '2005-07-12', 'Hải Phòng', 'Đang học', 'Nữ', 'thid@gmail.com', '111222333', 'd.jpg'),
                                                                                                                                                         (1, 3, 1, 'Nguyen Van E', '0905555666', '2003-09-05', 'Cần Thơ', 'Đang học', 'Nam', 'vane@gmail.com', '222333444', 'e.jpg'),
                                                                                                                                                         (1, 3, 1, 'Hoang Thi F', '0906666777', '2004-11-19', 'Huế', 'Đang học', 'Nữ', 'thif@gmail.com', '333444555', 'f.jpg'),
                                                                                                                                                         (1, 1, 2, 'Tran Van G', '0907777888', '2004-04-02', 'Quảng Ninh', 'Đang học', 'Nam', 'vang@gmail.com', '444555666', 'g.jpg'),
                                                                                                                                                         (1, 2, 4, 'Le Thi H', '0908888999', '2005-08-14', 'Bình Dương', 'Đang học', 'Nữ', 'thih@gmail.com', '555666777', 'h.jpg'),
                                                                                                                                                         (1, 4, 4, 'Pham Van I', '0909999000', '2003-12-25', 'Hà Nam', 'Đang học', 'Nam', 'vani@gmail.com', '666777888', 'i.jpg'),
                                                                                                                                                         (1, 4, 5, 'Vu Thi J', '0910000111', '2005-06-07', 'Thanh Hóa', 'Đang học', 'Nữ', 'thij@gmail.com', '777888999', 'j.jpg'),
                                                                                                                                                         (1, 2, 3, 'Ngo Van K', '0911111222', '2004-10-11', 'Nghệ An', 'Đang học', 'Nam', 'vank@gmail.com', '888999000', 'k.jpg'),
                                                                                                                                                         (1, 1, 2, 'Dang Thi L', '0912222333', '2004-01-20', 'Bắc Giang', 'Đang học', 'Nữ', 'thil@gmail.com', '999000111', 'l.jpg'),
                                                                                                                                                         (1, 3, 1, 'Nguyen Van M', '0913333444', '2005-02-16', 'Hà Tĩnh', 'Đang học', 'Nam', 'vanm@gmail.com', '123123123', 'm.jpg'),
                                                                                                                                                         (1, 3, 3, 'Pham Thi N', '0914444555', '2004-07-30', 'Thái Bình', 'Đang học', 'Nữ', 'thin@gmail.com', '234234234', 'n.jpg'),
                                                                                                                                                         (1, 2, 2, 'Tran Van O', '0915555666', '2003-03-18', 'Quảng Ngãi', 'Đang học', 'Nam', 'vano@gmail.com', '345345345', 'o.jpg'),
                                                                                                                                                         (1, 5, 4, 'Le Thi P', '0916666777', '2004-09-09', 'Kon Tum', 'Đang học', 'Nữ', 'thip@gmail.com', '456456456', 'p.jpg'),
                                                                                                                                                         (1, 5, 5, 'Nguyen Van Q', '0917777888', '2005-05-21', 'Bạc Liêu', 'Đang học', 'Nam', 'vanq@gmail.com', '567567567', 'q.jpg'),
                                                                                                                                                         (1, 1, 1, 'Hoang Thi R', '0918888999', '2004-08-13', 'Sóc Trăng', 'Đang học', 'Nữ', 'thir@gmail.com', '678678678', 'r.jpg'),
                                                                                                                                                         (1, 4, 2, 'Pham Van S', '0919999000', '2003-11-02', 'Gia Lai', 'Đang học', 'Nam', 'vans@gmail.com', '789789789', 's.jpg'),
                                                                                                                                                         (1, 3, 5, 'Vu Thi T', '0920000111', '2005-12-01', 'Vĩnh Long', 'Đang học', 'Nữ', 'thit@gmail.com', '890890890', 't.jpg'),
                                                                                                                                                         (1, 2, 3, 'Ngo Van U', '0921111222', '2004-06-06', 'Trà Vinh', 'Đang học', 'Nam', 'vanu@gmail.com', '901901901', 'u.jpg'),
                                                                                                                                                         (1, 5, 4, 'Dang Thi V', '0922222333', '2005-02-27', 'Tây Ninh', 'Đang học', 'Nữ', 'thiv@gmail.com', '112112112', 'v.jpg'),
                                                                                                                                                         (1, 4, 5, 'Nguyen Van X', '0923333444', '2004-04-23', 'Lâm Đồng', 'Đang học', 'Nam', 'vanx@gmail.com', '223223223', 'x.jpg');

-- 2
INSERT INTO Lop (MaGV, MaNganh, TenLop, SoLuongSV) VALUES
                                                       (1, 1, 'CNTT01', 50),
                                                       (2, 2, 'CNTT02', 45),
                                                       (3, 3, 'CNTT03', 60),
                                                       (4, 1, 'CNTT04', 55),
                                                       (5, 2, 'CNTT05', 40),

                                                       (6, 3, 'KT01', 65),
                                                       (7, 1, 'KT02', 50),
                                                       (8, 2, 'KT03', 45),
                                                       (9, 3, 'KT04', 60),
                                                       (10, 1, 'KT05', 55);

-- 3
INSERT INTO GiangVien
(MaTK, MaKhoa, TenGV, NgaySinhGV, GioiTinhGV, SoDienThoai, Email, TrangThai, AnhDaiDienGV) VALUES
                                                                                               (1, 1, 'Nguyễn Văn An', '1980-03-15', 'Nam', '0912345678', 'an.nguyen@univ.edu.vn', 'Đang công tác', 'an.jpg'),
                                                                                               (1, 1, 'Trần Thị Bình', '1982-07-22', 'Nữ', '0923456789', 'binh.tran@univ.edu.vn', 'Đang công tác', 'binh.jpg'),
                                                                                               (1, 1, 'Lê Quang Huy', '1985-05-10', 'Nam', '0934567890', 'huy.le@univ.edu.vn', 'Đang công tác', 'huy.jpg'),
                                                                                               (1, 1, 'Phạm Minh Châu', '1983-09-30', 'Nữ', '0945678901', 'chau.pham@univ.edu.vn', 'Đang nghỉ phép', 'chau.jpg'),
                                                                                               (1, 1, 'Đỗ Thị Thu Hà', '1987-01-25', 'Nữ', '0956789012', 'ha.do@univ.edu.vn', 'Đang công tác', 'ha.jpg'),
                                                                                               (1, 2, 'Ngô Văn Dũng', '1979-11-12', 'Nam', '0967890123', 'dung.ngo@univ.edu.vn', 'Đang công tác', 'dung.jpg'),
                                                                                               (1, 2, 'Vũ Thị Mai', '1986-04-18', 'Nữ', '0978901234', 'mai.vu@univ.edu.vn', 'Đang công tác', 'mai.jpg'),
                                                                                               (1, 2, 'Bùi Anh Tuấn', '1984-08-05', 'Nam', '0989012345', 'tuan.bui@univ.edu.vn', 'Đang nghỉ phép', 'tuan.jpg'),
                                                                                               (1, 2, 'Hoàng Lan Phương', '1988-02-14', 'Nữ', '0990123456', 'phuong.hoang@univ.edu.vn', 'Đang công tác', 'phuong.jpg'),
                                                                                               (1, 2, 'Phan Văn Khánh', '1978-12-20', 'Nam', '0901234567', 'khanh.phan@univ.edu.vn', 'Đang công tác', 'khanh.jpg');

-- 4
INSERT INTO Khoa (TenKHoa, Email, DiaChi) VALUES
                                              ('Cong Nghe Thong Tin', 'cntt@khoa.edu', '123 Duong 1'),
                                              ('Kinh Te', 'kinhte@khoa.edu', '456 Duong 2');
-- 5
INSERT INTO Nganh (MaKhoa, TenNganh) VALUES
                                         (1, 'Hệ thống thông tin'),
                                         (1, 'Mạng máy tính'),
                                         (1, 'Trí tuệ nhân tạo'),
                                         (1, 'Khoa học dữ liệu'),
                                         (1, 'An toàn thông tin'),
                                         (2, 'Tài chính'),
                                         (2, 'Kinh tế quốc tế'),
                                         (2, 'Quản lý kinh tế'),
                                         (2, 'Kế toán'),
                                         (2, 'Ngân hàng');

-- 6
INSERT INTO ChuyenNganh (MaNganh, TenCN) VALUES
                                             (1, 'Công nghệ phần mềm'),         -- Hệ thống thông tin
                                             (2, 'Tài chính công'),             -- Tài chính
                                             (3, 'Kiến trúc công trình dân dụng'), -- Xây dựng dân dụng (nếu em muốn bỏ, có thể giữ ngành khác)
                                             (4, 'Hệ thống nhúng'),             -- Mạng máy tính
                                             (5, 'Đầu tư quốc tế'),             -- Kinh tế quốc tế
                                             (6, 'Xây dựng hạ tầng kỹ thuật'),  -- Xây dựng cầu đường (nếu vẫn giữ ngành xây dựng)
                                             (7, 'Khoa học dữ liệu nâng cao'),  -- Trí tuệ nhân tạo
                                             (8, 'Bảo hiểm và quản lý rủi ro'), -- Quản lý kinh tế
                                             (9, 'Quản lý dự án'),              -- Kế toán
                                             (10, 'Bảo mật mạng');              -- Ngân hàng


-- 7
INSERT INTO HocPhiTinChi (MaNganh, SoTienMotTinChi, HocKy, Nam) VALUES
                                                                    (1, 480000, 1, '2025'), -- Hệ thống thông tin
                                                                    (2, 420000, 1, '2025'), -- Tài chính
                                                                    (3, 470000, 2, '2025'), -- Xây dựng dân dụng
                                                                    (4, 500000, 1, '2025'), -- Mạng máy tính
                                                                    (5, 430000, 2, '2025'), -- Kinh tế quốc tế
                                                                    (6, 510000, 2, '2025'), -- Trí tuệ nhân tạo
                                                                    (7, 410000, 1, '2025'), -- Quản lý kinh tế
                                                                    (8, 520000, 1, '2025'), -- Khoa học dữ liệu
                                                                    (9, 400000, 2, '2025'), -- Kế toán
                                                                    (10, 415000, 2, '2025'); -- Ngân hàng

-- 8
INSERT INTO HocPhiSV (MaSV, HocKy, Nam, TongHocPhi, DaThu, TrangThai) VALUES
                                                                          (1, 'HK1', '2025', 12000000, 6000000, 'Chưa đóng đủ'),
                                                                          (2, 'HK1', '2025', 11500000, 11500000, 'Đã đóng đủ'),
                                                                          (3, 'HK2', '2025', 11000000, 5000000, 'Chưa đóng đủ'),
                                                                          (4, 'HK2', '2025', 12500000, 12500000, 'Đã đóng đủ'),
                                                                          (5, 'HK1', '2025', 10000000, 10000000, 'Đã đóng đủ'),
                                                                          (6, 'HK1', '2025', 11800000, 6000000, 'Chưa đóng đủ'),
                                                                          (7, 'HK2', '2025', 12200000, 12200000, 'Đã đóng đủ'),
                                                                          (8, 'HK2', '2025', 11900000, 8000000, 'Chưa đóng đủ'),
                                                                          (9, 'HK1', '2025', 11300000, 11300000, 'Đã đóng đủ'),
                                                                          (10, 'HK1', '2025', 11700000, 5000000, 'Chưa đóng đủ'),
                                                                          (11, 'HK2', '2025', 12100000, 12100000, 'Đã đóng đủ'),
                                                                          (12, 'HK2', '2025', 11400000, 7000000, 'Chưa đóng đủ'),
                                                                          (13, 'HK1', '2025', 11600000, 11600000, 'Đã đóng đủ'),
                                                                          (14, 'HK1', '2025', 10900000, 4000000, 'Chưa đóng đủ'),
                                                                          (15, 'HK2', '2025', 12300000, 12300000, 'Đã đóng đủ'),
                                                                          (16, 'HK2', '2025', 11200000, 6000000, 'Chưa đóng đủ'),
                                                                          (17, 'HK1', '2025', 12400000, 12400000, 'Đã đóng đủ'),
                                                                          (18, 'HK1', '2025', 10800000, 5000000, 'Chưa đóng đủ'),
                                                                          (19, 'HK2', '2025', 12600000, 12600000, 'Đã đóng đủ'),
                                                                          (20, 'HK2', '2025', 11100000, 6000000, 'Chưa đóng đủ'),
                                                                          (21, 'HK1', '2025', 11900000, 11900000, 'Đã đóng đủ'),
                                                                          (22, 'HK1', '2025', 11500000, 7000000, 'Chưa đóng đủ'),
                                                                          (23, 'HK2', '2025', 12000000, 12000000, 'Đã đóng đủ');

-- 9
INSERT INTO KhoaHoc (MaCKDT, TenKhoaHoc, NienKhoaHoc) VALUES
                                                          (1, 'K21', '2021-2025'),
                                                          (1, 'K22', '2022-2026'),
                                                          (1, 'K23', '2023-2027'),

                                                          (2, 'K24', '2024-2028'),
                                                          (2, 'K25', '2025-2029');


-- 10
INSERT INTO ChuKyDaoTao (NamBatDau, NamKetThuc) VALUES
                                                    ('2020','2024'),
                                                    ('2024', '2028');

-- 11
-- Sinh viên đăng ký 3 nhóm học phần
INSERT INTO DangKy (MaNHP, MaSV) VALUES
                                     (1, 1), (2, 1), (3, 1),
                                     (4, 2), (5, 2), (6, 2),
                                     (7, 3), (8, 3), (9, 3),
                                     (10, 4), (11, 4), (12, 4),
                                     (13, 5), (14, 5), (15, 5),
                                     (16, 6), (1, 6), (2, 6),
                                     (3, 7), (4, 7), (5, 7),
                                     (6, 8), (7, 8), (8, 8),
                                     (9, 9), (10, 9), (11, 9),
                                     (12, 10), (13, 10), (14, 10),
                                     (15, 11), (16, 11), (1, 11),
                                     (2, 12), (3, 12), (4, 12),
                                     (5, 13), (6, 13), (7, 13),
                                     (8, 14), (9, 14), (10, 14),
                                     (11, 15), (12, 15), (13, 15),
                                     (14, 16), (15, 16), (16, 16),
                                     (1, 17), (2, 17), (3, 17),
                                     (4, 18), (5, 18), (6, 18),
                                     (7, 19), (8, 19), (9, 19),
                                     (10, 20), (11, 20), (12, 20),
                                     (13, 21), (14, 21), (15, 21),
                                     (16, 22), (1, 22), (2, 22),
                                     (3, 23), (4, 23), (5, 23);


-- 12
INSERT INTO NhomHocPhan (MaGV, MaHocPhan, HocKy, Nam, SiSo) VALUES
                                                                (1, 1, 1, 2025, 50),
                                                                (1, 2, 1, 2025, 45),
                                                                (2, 3, 2, 2025, 55),
                                                                (2, 4, 2, 2025, 60),
                                                                (3, 5, 1, 2025, 40),
                                                                (3, 6, 1, 2025, 50),
                                                                (4, 7, 2, 2025, 48),
                                                                (4, 8, 2, 2025, 52),
                                                                (5, 9, 1, 2025, 47),
                                                                (5, 10, 2, 2025, 53),
                                                                (6, 11, 1, 2025, 60),
                                                                (6, 12, 2, 2025, 58),
                                                                (7, 13, 1, 2025, 42),
                                                                (7, 14, 2, 2025, 50),
                                                                (8, 15, 1, 2025, 45),
                                                                (8, 16, 2, 2025, 45);


-- 13
INSERT INTO LichHoc (MaLH, MaPH, MaNHP, Thu, TietBatDau, TuNgay, DenNgay, TietKetThuc, SoTiet) VALUES
                                                                                                   (1, 1, 1, 2, 1, '2025-02-17', '2025-06-01', 3, 3),
                                                                                                   (2, 2, 2, 3, 4, '2025-02-17', '2025-06-01', 6, 3),
                                                                                                   (3, 3, 3, 4, 1, '2025-02-17', '2025-06-01', 2, 2),
                                                                                                   (4, 4, 4, 5, 7, '2025-02-17', '2025-06-01', 9, 3),
                                                                                                   (5, 5, 5, 6, 1, '2025-02-17', '2025-06-01', 3, 3),
                                                                                                   (6, 6, 6, 2, 4, '2025-02-17', '2025-06-01', 6, 3),
                                                                                                   (7, 7, 7, 3, 7, '2025-02-17', '2025-06-01', 9, 3),
                                                                                                   (8, 8, 8, 4, 1, '2025-02-17', '2025-06-01', 3, 3),
                                                                                                   (9, 9, 9, 5, 4, '2025-02-17', '2025-06-01', 6, 3),
                                                                                                   (10, 10, 10, 6, 7, '2025-02-17', '2025-06-01', 9, 3),
                                                                                                   (11, 11, 11, 2, 1, '2025-02-17', '2025-06-01', 2, 2),
                                                                                                   (12, 12, 12, 3, 4, '2025-02-17', '2025-06-01', 6, 3),
                                                                                                   (13, 13, 13, 4, 7, '2025-02-17', '2025-06-01', 9, 3),
                                                                                                   (14, 14, 14, 5, 1, '2025-02-17', '2025-06-01', 3, 3),
                                                                                                   (15, 15, 15, 6, 4, '2025-02-17', '2025-06-01', 6, 3),
                                                                                                   (16, 16, 16, 2, 7, '2025-02-17', '2025-06-01', 9, 3);

-- 14
INSERT INTO PhongHoc (TenPH, LoaiPH, CoSo, SucChua, TinhTrang) VALUES
                                                                   ('A101', 'Lý thuyết', 'Cơ sở A', 60, 'Sẵn sàng'),
                                                                   ('A102', 'Lý thuyết', 'Cơ sở A', 50, 'Sẵn sàng'),
                                                                   ('A103', 'Thực hành', 'Cơ sở A', 45, 'Sẵn sàng'),
                                                                   ('A104', 'Thực hành', 'Cơ sở A', 40, 'Bảo trì'),
                                                                   ('A201', 'Lý thuyết', 'Cơ sở A', 70, 'Sẵn sàng'),
                                                                   ('A202', 'Thực hành', 'Cơ sở A', 50, 'Sẵn sàng'),
                                                                   ('A203', 'Lý thuyết', 'Cơ sở A', 80, 'Sẵn sàng'),
                                                                   ('A204', 'Thực hành', 'Cơ sở A', 45, 'Sẵn sàng'),

                                                                   ('B101', 'Lý thuyết', 'Cơ sở B', 60, 'Sẵn sàng'),
                                                                   ('B102', 'Thực hành', 'Cơ sở B', 50, 'Sẵn sàng'),
                                                                   ('B103', 'Lý thuyết', 'Cơ sở B', 70, 'Sẵn sàng'),
                                                                   ('B104', 'Thực hành', 'Cơ sở B', 40, 'Bảo trì'),
                                                                   ('B201', 'Lý thuyết', 'Cơ sở B', 80, 'Sẵn sàng'),
                                                                   ('B202', 'Thực hành', 'Cơ sở B', 55, 'Sẵn sàng'),
                                                                   ('B203', 'Lý thuyết', 'Cơ sở B', 60, 'Sẵn sàng'),
                                                                   ('B204', 'Thực hành', 'Cơ sở B', 50, 'Sẵn sàng');

-- 15
INSERT INTO HocPhan (MaHPTruoc, TenHP, SoTinChi, HeSoHocPhan, SoTietLyThuyet, SoTietThucHanh) VALUES
                                                                                                  (NULL, 'Lập trình hướng đối tượng', 3, 1.0, 30, 15),     -- Công nghệ phần mềm
                                                                                                  (NULL, 'Kỹ thuật vi điều khiển', 3, 1.0, 30, 15),       -- Hệ thống nhúng
                                                                                                  (NULL, 'Khai phá dữ liệu', 3, 1.0, 30, 15),             -- Khoa học dữ liệu
                                                                                                  (NULL, 'Phân tích dữ liệu với Python', 3, 1.0, 30, 15), -- Phân tích dữ liệu lớn
                                                                                                  (NULL, 'An toàn hệ thống mạng', 3, 1.0, 30, 15),        -- An ninh mạng
                                                                                                  (NULL, 'Quản lý ngân sách công', 3, 1.0, 45, 0),        -- Tài chính công
                                                                                                  (NULL, 'Thương mại quốc tế', 3, 1.0, 45, 0),            -- Kinh tế đối ngoại
                                                                                                  (NULL, 'Quản trị chiến lược', 3, 1.0, 45, 0),           -- Quản trị kinh doanh
                                                                                                  (NULL, 'Nguyên lý kế toán', 3, 1.0, 45, 0),             -- Kế toán kiểm toán
                                                                                                  (NULL, 'Nghiệp vụ ngân hàng', 3, 1.0, 45, 0),           -- Ngân hàng đầu tư
                                                                                                  (NULL, 'Triết học Mác – Lênin', 2, 1.0, 30, 0),
                                                                                                  (11, 'Kinh tế chính trị Mác – Lênin', 2, 1.0, 30, 0),
                                                                                                  (12, 'Chủ nghĩa xã hội khoa học', 2, 1.0, 30, 0),
                                                                                                  (13, 'Lịch sử Đảng Cộng sản Việt Nam', 2, 1.0, 30, 0),
                                                                                                  (14, 'Tư tưởng Hồ Chí Minh', 2, 1.0, 30, 0);

-- 16
INSERT INTO ChiTietDiem (MaSV, MaHP, HocKy, Nam) VALUES
                                                     (1, 1, 1, '2025'), (1, 2, 1, '2025'), (1, 3, 2, '2025'),
                                                     (2, 4, 1, '2025'), (2, 5, 2, '2025'), (2, 6, 1, '2025'),
                                                     (3, 7, 1, '2025'), (3, 8, 2, '2025'), (3, 9, 2, '2025'),
                                                     (4, 10, 1, '2025'), (4, 11, 2, '2025'), (4, 12, 1, '2025'),
                                                     (5, 13, 2, '2025'), (5, 14, 1, '2025'), (5, 1, 2, '2025'),
                                                     (6, 2, 1, '2025'), (6, 3, 2, '2025'), (6, 4, 1, '2025'),
                                                     (7, 5, 2, '2025'), (7, 6, 1, '2025'), (7, 7, 2, '2025'),
                                                     (8, 8, 1, '2025'), (8, 9, 2, '2025'), (8, 10, 1, '2025'),
                                                     (9, 11, 2, '2025'), (9, 12, 1, '2025'), (9, 13, 2, '2025'),
                                                     (10, 14, 1, '2025'), (10, 1, 2, '2025'), (10, 2, 1, '2025');



-- 17
INSERT INTO KetQua (MaCTD, DiemHe4, DiemHe10) VALUES
                                                  (1, 3.0, 7.5),
                                                  (2, 3.2, 8.0),
                                                  (3, 2.7, 6.8),
                                                  (4, 3.6, 9.0),
                                                  (5, 2.9, 7.2),
                                                  (6, 3.4, 8.5),
                                                  (7, 2.4, 6.0),
                                                  (8, 3.1, 7.8),
                                                  (9, 3.3, 8.2),
                                                  (10, 3.7, 9.1);

-- 18
INSERT INTO DiemThi (MaCTD, DiemSo) VALUES
                                        (1, 7.5), (2, 8.0), (3, 6.8),
                                        (4, 9.0), (5, 7.2), (6, 8.5),
                                        (7, 6.0), (8, 7.8), (9, 8.2),
                                        (10, 9.1);


-- 19
INSERT INTO DiemQuaTrinh (MaCTD, DiemSo) VALUES
                                             (1, 6.8), (2, 7.5), (3, 6.0),
                                             (4, 8.2), (5, 6.7), (6, 7.8),
                                             (7, 5.9), (8, 7.0), (9, 7.3),
                                             (10, 8.5);

-- 20
INSERT INTO CotDiem (MaDQT, MaHP, TenCotDiem, DiemSo, HeSo) VALUES
                                                                (1, 1, 'Kiểm tra', 7.2, 1),
                                                                (2, 2, 'Kiểm tra', 6.5, 1),
                                                                (3, 3, 'Kiểm tra', 8.0, 1),
                                                                (4, 4, 'Kiểm tra', 7.8, 1),
                                                                (5, 5, 'Kiểm tra', 6.9, 1),
                                                                (6, 6, 'Kiểm tra', 7.4, 1),
                                                                (7, 7, 'Kiểm tra', 8.1, 1),
                                                                (8, 8, 'Kiểm tra', 7.0, 1),
                                                                (9, 9, 'Kiểm tra', 6.7, 1),
                                                                (10, 10, 'Kiểm tra', 8.3, 1);


-- 21
INSERT INTO HocPhiHocPhan (MaSV, MaHP, TongTien) VALUES
                                                     (1, 1, 1500000),
                                                     (1, 2, 1800000),
                                                     (2, 1, 1500000),
                                                     (2, 3, 2000000),
                                                     (3, 2, 1800000),
                                                     (3, 4, 2200000),
                                                     (4, 3, 2000000),
                                                     (4, 5, 1700000),
                                                     (5, 1, 1500000),
                                                     (5, 4, 2200000),
                                                     (6, 2, 1800000),
                                                     (6, 5, 1700000),
                                                     (7, 3, 2000000),
                                                     (7, 6, 1600000),
                                                     (8, 1, 1500000),
                                                     (8, 6, 1600000),
                                                     (9, 4, 2200000),
                                                     (9, 2, 1800000),
                                                     (10, 5, 1700000),
                                                     (10, 6, 1600000);



-- 22
INSERT INTO CaThi (MaHP, MaPH, Thu, ThoiGianBatDau, ThoiLuong) VALUES
                                                                   (1, 1, 2, '08:00:00', 90),
                                                                   (2, 2, 3, '09:30:00', 120),
                                                                   (3, 3, 4, '13:00:00', 90),
                                                                   (4, 4, 5, '14:30:00', 60),
                                                                   (5, 5, 6, '08:00:00', 120),
                                                                   (6, 6, 7, '10:00:00', 90),
                                                                   (7, 7, 2, '15:00:00', 60),
                                                                   (8, 8, 3, '07:30:00', 90),
                                                                   (9, 9, 4, '16:00:00', 120),
                                                                   (10, 10, 5, '09:00:00', 90);


-- 23
INSERT INTO CaThi_SinhVien (MaCT, MaSV) VALUES
                                            (1, 1), (1, 2), (1, 3),
                                            (2, 4), (2, 5), (2, 6),
                                            (3, 7), (3, 8), (3, 9);

-- 24
INSERT INTO ChuongTrinhDaoTao_HocPhan (MaCTDT, MaHP) VALUES
-- CTDT 1: Công nghệ thông tin
(1, 1),
(1, 2),
(1, 3),
(1, 4),
(1, 5),
-- CTDT 2: Kinh tế
(2, 6),
(2, 7),
(2, 8),
(2, 9),
(2, 10),
-- Các môn đại cương cho CTDT 1
(1, 11),
(1, 12),
(1, 13),
(1, 14),
(1, 15),
-- Các môn đại cương cho CTDT 2
(2, 11),
(2, 12),
(2, 13),
(2, 14),
(2, 15);

-- 25
INSERT INTO ChuongTrinhDaoTao (MaNganh, TenCTDT, LoaiHinhDT, TrinhDo) VALUES
                                                                          (1, 'Công nghệ thông tin - Kỹ sư', 'Chính quy', 'Kỹ sư'),
                                                                          (2, 'Kinh tế - Cử nhân', 'Chính quy', 'Cử nhân');


-- 26
INSERT INTO TaiKhoan (MaTK, MaNQ, TenDangNhap, MatKhau) VALUES
    (1, 1, 'admin', '123456');


-- 27
INSERT INTO NhomQuyen (TenNhomQuyen) VALUES
    ('Admin');

-- 28
INSERT INTO ChiTietQuyen (MaCN, MaNQ, HanhDong) VALUES
                                                    (1, 1, 'view'), (1, 1, 'insert'), (1, 1, 'update'), (1, 1, 'delete'),
                                                    (2, 1, 'view'), (2, 1, 'insert'), (2, 1, 'update'), (2, 1, 'delete'),
                                                    (3, 1, 'view'), (3, 1, 'insert'), (3, 1, 'update'), (3, 1, 'delete'),
                                                    (4, 1, 'view'), (4, 1, 'insert'), (4, 1, 'update'), (4, 1, 'delete'),
                                                    (5, 1, 'view'), (5, 1, 'insert'), (5, 1, 'update'), (5, 1, 'delete'),
                                                    (6, 1, 'view'), (6, 1, 'insert'), (6, 1, 'update'), (6, 1, 'delete'),
                                                    (7, 1, 'view'), (7, 1, 'insert'), (7, 1, 'update'), (7, 1, 'delete'),
                                                    (8, 1, 'view'), (8, 1, 'insert'), (8, 1, 'update'), (8, 1, 'delete'),
                                                    (9, 1, 'view'), (9, 1, 'insert'), (9, 1, 'update'), (9, 1, 'delete'),
                                                    (10, 1, 'view'), (10, 1, 'insert'), (10, 1, 'update'), (10, 1, 'delete'),
                                                    (11, 1, 'view'), (11, 1, 'insert'), (11, 1, 'update'), (11, 1, 'delete');

-- 29
INSERT INTO ChucNang (TenChucNang) VALUES
                                       ('trangchu'),
                                       ('sinhvien'),
                                       ('giangvien'),
                                       ('khoa'),
                                       ('nganh'),
                                       ('chuongtrinhdaotao'),
                                       ('hocphan'),
                                       ('phonghoc'),
                                       ('tochucthi'),
                                       ('nhapdiem'),
                                       ('sinhvien'),
                                       ('phanquyen'),
                                       ('thongke');

-- ---------------------Khóa Ngoại ---------------------------

ALTER TABLE `SinhVien`
    ADD CONSTRAINT SinhVien_Lop FOREIGN KEY (MaLop) REFERENCES `Lop`(MaLop);

ALTER TABLE `Lop`
    ADD CONSTRAINT Lop_GiangVien FOREIGN KEY (MaGV) REFERENCES `GiangVien`(MaGV);

ALTER TABLE `Lop`
    ADD CONSTRAINT Lop_Nganh FOREIGN KEY (MaNganh) REFERENCES `Nganh`(MaNganh);

ALTER TABLE `Nganh`
    ADD CONSTRAINT `Nganh_Khoa` FOREIGN KEY (MaKhoa) REFERENCES `Khoa`(MaKhoa);

ALTER TABLE `GiangVien`
    ADD CONSTRAINT `GiangVien_Khoa` FOREIGN KEY (MaKhoa) REFERENCES `Khoa`(MaKhoa);

ALTER TABLE `ChuyenNganh`
    ADD CONSTRAINT `ChuyenNganh_Nganh` FOREIGN KEY (MaNganh) REFERENCES `Nganh`(MaNganh);

ALTER TABLE `HocPhiTinChi`
    ADD CONSTRAINT `HocPhiTinChi_Nganh` FOREIGN KEY (MaNganh) REFERENCES `Nganh`(MaNganh);

ALTER TABLE `HocPhiSV`
    ADD CONSTRAINT `HocPhiSV_SinhVien` FOREIGN KEY (MaSV) REFERENCES `SinhVien`(MaSV);

ALTER TABLE `SinhVien`
    ADD CONSTRAINT `SinhVien_KhoaHoc` FOREIGN KEY (MaKhoaHoc) REFERENCES `KhoaHoc`(MaKhoaHoc);

ALTER TABLE `KhoaHoc`
    ADD CONSTRAINT `KhoaHoc_ChuKyDaoTao` FOREIGN KEY (MaCKDT) REFERENCES `ChuKyDaoTao`(MaCKDT);

ALTER TABLE `DangKy`
    ADD CONSTRAINT `DangKy_SinhVien` FOREIGN KEY (MaSV) REFERENCES `SinhVien`(MaSV),
    ADD CONSTRAINT `DangKy_NhomHocPhan` FOREIGN KEY (MaNHP) REFERENCES `NhomHocPhan`(MaNHP);

ALTER TABLE `LichHoc`
    ADD CONSTRAINT `LichHoc_PhongHoc` FOREIGN KEY (MaPH) REFERENCES `PhongHoc`(MaPH);

ALTER TABLE `LichHoc`
    ADD CONSTRAINT `LichHoc_NhomHocPhan` FOREIGN KEY (MaNHP) REFERENCES `NhomHocPhan`(MaNHP);

ALTER TABLE `CaThi`
    ADD CONSTRAINT `CaThi_PhongHoc` FOREIGN KEY (MaPH) REFERENCES `PhongHoc`(MaPH),
    ADD CONSTRAINT `CaThi_HocPhan` FOREIGN KEY (MaHP) REFERENCES `HocPhan`(MaHP);

ALTER TABLE `CaThi_SinhVien`
    ADD CONSTRAINT `CaThi_SinhVien_CaThi` FOREIGN KEY (MaCT) REFERENCES `CaThi`(MaCT),
    ADD CONSTRAINT `CaThi_SinhVien_SinhVien` FOREIGN KEY (MaSV) REFERENCES `SinhVien`(MaSV);

ALTER TABLE `HocPhiHocPhan`
    ADD CONSTRAINT `HocPhiHocPhan_HocPhan` FOREIGN KEY (MaHP) REFERENCES `HocPhan`(MaHP),
    ADD CONSTRAINT `HocPhiHocPhan_SinhVien` FOREIGN KEY (MaSV) REFERENCES `SinhVien`(MaSV);

ALTER TABLE `ChiTietDiem`
    ADD CONSTRAINT `ChiTietDiem_HocPhan` FOREIGN KEY (MaHP) REFERENCES `HocPhan`(MaHP),
    ADD CONSTRAINT `ChiTietDiem_SinhVien` FOREIGN KEY (MaSV) REFERENCES `SinhVien`(MaSV);

ALTER TABLE `DiemThi`
    ADD CONSTRAINT `DiemThi_ChiTietDiem` FOREIGN KEY (MaCTD) REFERENCES `ChiTietDiem`(MaCTD);

ALTER TABLE `KetQua`
    ADD CONSTRAINT `KetQua_ChiTietDiem` FOREIGN KEY (MaCTD) REFERENCES `ChiTietDiem`(MaCTD);

ALTER TABLE `DiemQuaTrinh`
    ADD CONSTRAINT `DiemQuaTrinh_ChiTietDiem` FOREIGN KEY (MaCTD) REFERENCES `ChiTietDiem`(MaCTD);

ALTER TABLE `CotDiem`
    ADD CONSTRAINT `CotDiem_DiemQuaTrinh` FOREIGN KEY (MaDQT) REFERENCES `DiemQuaTrinh`(MaDQT);

ALTER TABLE `SinhVien`
    ADD CONSTRAINT `SinhVien_TaiKhoan` FOREIGN KEY (MaTK) REFERENCES `TaiKhoan`(MaTK);

ALTER TABLE `GiangVien`
    ADD CONSTRAINT `GiangVien_TaiKhoan` FOREIGN KEY (MaTK) REFERENCES `TaiKhoan`(MaTK);

ALTER TABLE `TaiKhoan`
    ADD CONSTRAINT `TaiKhoan_NhomQuyen` FOREIGN KEY (MaNQ) REFERENCES `NhomQuyen`(MaNQ);

ALTER TABLE `ChiTietQuyen`
    ADD CONSTRAINT `ChiTietQuyen_NhomQuyen` FOREIGN KEY (MaNQ) REFERENCES `NhomQuyen`(MaNQ),
    ADD CONSTRAINT `ChiTietQuyen_ChucNang` FOREIGN KEY (MaCN) REFERENCES `ChucNang`(MaCN);

ALTER TABLE `NhomHocPhan`
    ADD CONSTRAINT `NhomHocPhan_GiangVien` FOREIGN KEY (MaGV) REFERENCES `GiangVien`(MaGV);