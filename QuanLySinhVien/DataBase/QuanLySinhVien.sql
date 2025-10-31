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
                             NgaySinhGV VARCHAR(255),
                             GioiTinhGV VARCHAR(255),
                             SoDienThoai VARCHAR(255),
                             Email  VARCHAR(255),
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
                               `MaHP` INT,
			       `MaLichDK` INT DEFAULT 1,
				`MaLop` INT,
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
                           `Thu` VARCHAR(255),
                           `TietBatDau` INT,
                           `TuNgay` date,
                           `DenNgay` date,
                           `TietKetThuc` INT,
                           `SoTiet` INT,
			   `Type` VARCHAR(255) DEFAULT 'Lý thuyết',
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


-- 17
-- điểm thi chưa nhập
CREATE TABLE `KetQua` (
                          `MaKQ` INT AUTO_INCREMENT PRIMARY KEY,
                          `MaHP` INT,
                          `MaSV` INT,
                          `DiemThi` float DEFAULT 0,
                          `DiemHe4` float DEFAULT 0,
                          `DiemHe10` float DEFAULT 0,
                          `HocKy` INT,
                          `Nam` VARCHAR(255),
                          Status TINYINT DEFAULT 1
);

-- 19
CREATE TABLE `DiemQuaTrinh` (
                                `MaDQT` INT AUTO_INCREMENT PRIMARY KEY,
								`MaKQ` INT,
                                `DiemSo` float,
                                Status TINYINT DEFAULT 1
);

-- 20
CREATE TABLE `CotDiem` (
                           `MaCD` INT AUTO_INCREMENT PRIMARY KEY,
                           `MaDQT` INT,
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
                         `Thu` VARCHAR(255),
                         `ThoiGianBatDau` VARCHAR(255),
                         `ThoiLuong` VARCHAR(255),
						 `HocKy` INT,
						 `Nam` VARCHAR(255),
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
CREATE TABLE chuongtrinhdaotao_hocphan(
                                          MaCTDT INT,
                                          MaHP INT,
                                          PRIMARY KEY (MaCTDT, MaHP),
                                          Status TINYINT DEFAULT 1
);

-- 25
create table chuongtrinhdaotao(
                                  MaCTDT INT AUTO_INCREMENT PRIMARY KEY,
                                  MaCKDT INT, -- FK
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
			  Type VARCHAR(255) DEFAULT 'Quản trị viên',
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

-- 30
CREATE TABLE LichDangKy  (
                           MaLichDK INT AUTO_INCREMENT PRIMARY KEY ,
                           ThoiGianBatDau TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
			   ThoiGianKetThuc TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                           Status TINYINT DEFAULT 1
);


-- ------------------------INSERT-----------------------------------
-- 1
INSERT INTO SinhVien (MaTK, MaLop, MaKhoaHoc, TenSV, SoDienThoaiSV, NgaySinhSV, QueQuanSV, TrangThaiSV, GioiTinhSV, EmailSV, CCCDSV, AnhDaiDienSV) VALUES
(3, 2, 1, 'Nguyễn Ân', '0968188598', '24/06/1995', 'Bình Định', 'Đang học', 'Nam', 'an.sv@gmail.com', '528139535131', 'img/portrait/default.jpg'),
(5, 8, 1, 'Ngô Thị Thu', '0976545290', '27/10/1997', 'Vũng Tàu', 'Đang học', 'Nữ', 'ngo.thi.thu@gmail.com', '249229315668', 'img/portrait/default.jpg'),
(6, 4, 1, 'Trần Văn Anh', '0960877017', '13/12/1998', 'Huế', 'Đang học', 'Nam', 'tran.van.anh@gmail.com', '753662145005', 'img/portrait/default.jpg'),
(7, 6, 1, 'Dương Thị Lan', '0975025648', '18/03/2000', 'Nha Trang', 'Đang học', 'Nữ', 'duong.thi.lan@gmail.com', '254131630817', 'img/portrait/default.jpg'),
(8, 9, 1, 'Lý Thị Cúc', '0942677057', '16/05/2000', 'Phú Yên', 'Đang học', 'Nữ', 'ly.thi.cuc@gmail.com', '288784918817', 'img/portrait/default.jpg'),
(9, 9, 1, 'Võ Thị Nga', '0963229308', '01/04/1992', 'Hà Nội', 'Đang học', 'Nữ', 'vo.thi.nga@gmail.com', '139788401816', 'img/portrait/default.jpg'),
(10, 7, 1, 'Bùi Thị Lan', '0985119093', '04/04/2004', 'Cần Thơ', 'Đang học', 'Nữ', 'bui.thi.lan@gmail.com', '346597264307', 'img/portrait/default.jpg'),
(11, 10, 1, 'Bùi Thị Trang', '0975984367', '07/07/2001', 'Nha Trang', 'Đang học', 'Nữ', 'bui.thi.trang@gmail.com', '557048851125', 'img/portrait/default.jpg'),
(12, 2, 1, 'Bùi Văn Anh', '0938185337', '23/10/1990', 'Lạng Sơn', 'Đang học', 'Nam', 'bui.van.anh@gmail.com', '729358637684', 'img/portrait/default.jpg'),
(13, 5, 1, 'Dương Văn Bình', '0987631890', '20/07/2003', 'Phú Yên', 'Đang học', 'Nam', 'duong.van.binh@gmail.com', '952070995341', 'img/portrait/default.jpg'),
(14, 7, 1, 'Hoàng Văn Tuấn', '0954639950', '02/09/2002', 'Hải Phòng', 'Đang học', 'Nam', 'hoang.van.tuan@gmail.com', '728526660271', 'img/portrait/default.jpg'),
(15, 7, 1, 'Huỳnh Thị Oanh', '0910840481', '10/07/1992', 'Bình Định', 'Đang học', 'Nữ', 'huynh.thi.oanh@gmail.com', '402813743790', 'img/portrait/default.jpg'),
(16, 5, 1, 'Dương Văn Tuấn', '0930965419', '06/04/2004', 'Bình Định', 'Đang học', 'Nam', 'duong.van.tuan@gmail.com', '903107784770', 'img/portrait/default.jpg'),
(17, 1, 1, 'Dương Thị Hồng', '0934606438', '17/07/1997', 'Nha Trang', 'Đang học', 'Nữ', 'duong.thi.hong@gmail.com', '299969123142', 'img/portrait/default.jpg'),
(18, 3, 1, 'Đặng Thị Thu', '0957256873', '19/08/1991', 'Đà Nẵng', 'Đang học', 'Nữ', 'dang.thi.thu@gmail.com', '744566000316', 'img/portrait/default.jpg'),
(19, 4, 1, 'Dương Thị Trang', '0993660968', '18/02/1992', 'Thái Nguyên', 'Đang học', 'Nữ', 'duong.thi.trang@gmail.com', '472281120301', 'img/portrait/default.jpg'),
(20, 3, 1, 'Nguyễn Văn Bảo', '0919105948', '02/04/1990', 'Hải Phòng', 'Đang học', 'Nam', 'nguyen.van.bao@gmail.com', '374338315193', 'img/portrait/default.jpg'),
(21, 7, 1, 'Lê Văn An', '0927249571', '18/10/1995', 'Quảng Ngãi', 'Đang học', 'Nam', 'le.van.an@gmail.com', '116716159078', 'img/portrait/default.jpg'),
(22, 1, 1, 'Lý Văn Huy', '0945362244', '07/07/1997', 'Hải Phòng', 'Đang học', 'Nam', 'ly.van.huy@gmail.com', '874003265039', 'img/portrait/default.jpg'),
(23, 5, 1, 'Vũ Văn Đức', '0986667155', '14/10/2005', 'Hồ Chí Minh', 'Đang học', 'Nam', 'vu.van.duc@gmail.com', '942076918765', 'img/portrait/default.jpg'),
(24, 5, 2, 'Võ Văn Huy', '0985130154', '02/07/1992', 'Quảng Ngãi', 'Đang học', 'Nam', 'vo.van.huy@gmail.com', '970471255055', 'img/portrait/default.jpg'),
(25, 9, 2, 'Nguyễn Văn Đạt', '0988165182', '07/08/2001', 'Hà Nội', 'Đang học', 'Nam', 'nguyen.van.dat@gmail.com', '111410384072', 'img/portrait/default.jpg'),
(26, 9, 2, 'Phan Thị Mai', '0953457661', '01/12/1998', 'Nha Trang', 'Đang học', 'Nữ', 'phan.thi.mai@gmail.com', '249652349826', 'img/portrait/default.jpg'),
(27, 9, 2, 'Đỗ Văn Anh', '0956526100', '17/03/2002', 'Hà Nội', 'Đang học', 'Nam', 'do.van.anh@gmail.com', '088755768124', 'img/portrait/default.jpg'),
(28, 8, 2, 'Lê Thị Oanh', '0913782886', '22/11/1993', 'Quảng Ninh', 'Đang học', 'Nữ', 'le.thi.oanh@gmail.com', '836428813620', 'img/portrait/default.jpg'),
(29, 2, 2, 'Đặng Văn Huy', '0944505045', '18/11/1990', 'Vũng Tàu', 'Đang học', 'Nam', 'dang.van.huy@gmail.com', '716100597038', 'img/portrait/default.jpg'),
(30, 6, 2, 'Đỗ Văn Anh', '0968988097', '20/05/1999', 'Quảng Ngãi', 'Đang học', 'Nam', 'do.van.anh@gmail.com', '637120833939', 'img/portrait/default.jpg'),
(31, 6, 2, 'Vũ Văn Huy', '0967609213', '16/07/1995', 'Nha Trang', 'Đang học', 'Nam', 'vu.van.huy@gmail.com', '887951281121', 'img/portrait/default.jpg'),
(32, 1, 2, 'Phan Thị Lan', '0956643439', '11/05/2004', 'Hà Nội', 'Đang học', 'Nữ', 'phan.thi.lan@gmail.com', '759148636710', 'img/portrait/default.jpg'),
(33, 8, 2, 'Trần Thị Cúc', '0949835050', '15/09/1998', 'Quảng Ninh', 'Đang học', 'Nữ', 'tran.thi.cuc@gmail.com', '799876843417', 'img/portrait/default.jpg'),
(34, 2, 2, 'Ngô Thị Oanh', '0947556300', '17/06/1992', 'Quảng Ninh', 'Đang học', 'Nữ', 'ngo.thi.oanh@gmail.com', '758103976220', 'img/portrait/default.jpg'),
(35, 7, 2, 'Đỗ Văn Huy', '0919963209', '14/08/2003', 'Hải Phòng', 'Đang học', 'Nam', 'do.van.huy@gmail.com', '567476583871', 'img/portrait/default.jpg'),
(36, 3, 2, 'Dương Thị Trang', '0990404981', '17/04/2005', 'Lạng Sơn', 'Đang học', 'Nữ', 'duong.thi.trang@gmail.com', '972253106210', 'img/portrait/default.jpg'),
(37, 2, 2, 'Vũ Văn Huy', '0936606950', '24/09/1991', 'Quảng Ninh', 'Đang học', 'Nam', 'vu.van.huy@gmail.com', '468161421941', 'img/portrait/default.jpg'),
(38, 8, 2, 'Hồ Thị Cúc', '0917968115', '26/07/2000', 'Hồ Chí Minh', 'Đang học', 'Nữ', 'ho.thi.cuc@gmail.com', '971052568026', 'img/portrait/default.jpg'),
(39, 10, 2, 'Huỳnh Văn Huy', '0977161562', '20/08/1992', 'Cần Thơ', 'Đang học', 'Nam', 'huynh.van.huy@gmail.com', '916355009153', 'img/portrait/default.jpg'),
(40, 8, 2, 'Lý Văn Bảo', '0941593382', '05/03/1997', 'Hải Phòng', 'Đang học', 'Nam', 'ly.van.bao@gmail.com', '388893186791', 'img/portrait/default.jpg'),
(41, 9, 2, 'Phạm Văn Minh', '0947797190', '25/01/1999', 'Hà Nội', 'Đang học', 'Nam', 'pham.van.minh@gmail.com', '670702682587', 'img/portrait/default.jpg'),
(42, 9, 2, 'Nguyễn Văn Bình', '0949116879', '27/06/1992', 'Quảng Ninh', 'Đang học', 'Nam', 'nguyen.van.binh@gmail.com', '732817687083', 'img/portrait/default.jpg'),
(43, 7, 2, 'Võ Thị Oanh', '0999118050', '21/08/1994', 'Hà Nội', 'Đang học', 'Nữ', 'vo.thi.oanh@gmail.com', '405800119293', 'img/portrait/default.jpg'),
(44, 3, 3, 'Huỳnh Thị Lan', '0952482668', '16/01/1991', 'Đà Nẵng', 'Đang học', 'Nữ', 'huynh.thi.lan@gmail.com', '866429306409', 'img/portrait/default.jpg'),
(45, 4, 3, 'Đặng Thị Trang', '0951300021', '02/05/2000', 'Huế', 'Đang học', 'Nữ', 'dang.thi.trang@gmail.com', '566603610522', 'img/portrait/default.jpg'),
(46, 6, 3, 'Hoàng Văn Anh', '0943943811', '09/09/1999', 'Phú Yên', 'Đang học', 'Nam', 'hoang.van.anh@gmail.com', '074468842971', 'img/portrait/default.jpg'),
(47, 2, 3, 'Hồ Thị Mai', '0988910984', '09/02/1996', 'Đà Nẵng', 'Đang học', 'Nữ', 'ho.thi.mai@gmail.com', '283183080740', 'img/portrait/default.jpg'),
(48, 5, 3, 'Huỳnh Thị Bình', '0989703958', '12/04/1995', 'Vũng Tàu', 'Đang học', 'Nữ', 'huynh.thi.binh@gmail.com', '882214154933', 'img/portrait/default.jpg'),
(49, 7, 3, 'Đặng Thị Nga', '0939986732', '02/12/1996', 'Quảng Ngãi', 'Đang học', 'Nữ', 'dang.thi.nga@gmail.com', '985659665331', 'img/portrait/default.jpg'),
(50, 5, 3, 'Hồ Văn Minh', '0974541384', '24/07/2000', 'Quảng Ngãi', 'Đang học', 'Nam', 'ho.van.minh@gmail.com', '870426297802', 'img/portrait/default.jpg'),
(51, 9, 3, 'Hoàng Thị Trang', '0939662210', '18/11/1991', 'Huế', 'Đang học', 'Nữ', 'hoang.thi.trang@gmail.com', '657127190993', 'img/portrait/default.jpg'),
(52, 9, 3, 'Phạm Thị Oanh', '0972362015', '20/02/1998', 'Lạng Sơn', 'Đang học', 'Nữ', 'pham.thi.oanh@gmail.com', '166426123202', 'img/portrait/default.jpg'),
(53, 1, 3, 'Vũ Thị Ngọc', '0977818390', '14/08/2000', 'Nha Trang', 'Đang học', 'Nữ', 'vu.thi.ngoc@gmail.com', '060864690981', 'img/portrait/default.jpg'),
(54, 9, 3, 'Ngô Văn An', '0993289786', '16/05/2001', 'Phú Yên', 'Đang học', 'Nam', 'ngo.van.an@gmail.com', '608367516943', 'img/portrait/default.jpg'),
(55, 7, 3, 'Lê Thị Mai', '0998110612', '28/12/1993', 'Hà Nội', 'Đang học', 'Nữ', 'le.thi.mai@gmail.com', '039539881185', 'img/portrait/default.jpg'),
(56, 4, 3, 'Nguyễn Văn Hùng', '0933622049', '27/12/2002', 'Bình Định', 'Đang học', 'Nam', 'nguyen.van.hung@gmail.com', '739299531143', 'img/portrait/default.jpg'),
(57, 6, 3, 'Bùi Văn Anh', '0948235132', '09/02/2002', 'Thái Nguyên', 'Đang học', 'Nam', 'bui.van.anh@gmail.com', '334554567782', 'img/portrait/default.jpg'),
(58, 8, 3, 'Đặng Văn Tuấn', '0979373867', '03/05/1994', 'Phú Yên', 'Đang học', 'Nam', 'dang.van.tuan@gmail.com', '295510107255', 'img/portrait/default.jpg'),
(59, 3, 3, 'Phạm Văn Huy', '0997997231', '14/04/1990', 'Phú Yên', 'Đang học', 'Nam', 'pham.van.huy@gmail.com', '894643342630', 'img/portrait/default.jpg'),
(60, 4, 3, 'Ngô Văn Anh', '0924517078', '12/11/1995', 'Lạng Sơn', 'Đang học', 'Nam', 'ngo.van.anh@gmail.com', '458996509385', 'img/portrait/default.jpg'),
(61, 4, 3, 'Lý Văn Đức', '0931860270', '17/03/2005', 'Cần Thơ', 'Đang học', 'Nam', 'ly.van.duc@gmail.com', '771314174192', 'img/portrait/default.jpg'),
(62, 9, 3, 'Lê Thị Bình', '0934974402', '14/11/1990', 'Quảng Ninh', 'Đang học', 'Nữ', 'le.thi.binh@gmail.com', '082493735583', 'img/portrait/default.jpg'),
(63, 3, 3, 'Lê Thị Oanh', '0934385625', '24/02/1996', 'Phú Yên', 'Đang học', 'Nữ', 'le.thi.oanh@gmail.com', '277314520877', 'img/portrait/default.jpg'),
(64, 6, 4, 'Bùi Văn Khang', '0940888729', '16/10/2002', 'Hà Nội', 'Đang học', 'Nam', 'bui.van.khang@gmail.com', '524755200562', 'img/portrait/default.jpg'),
(65, 9, 4, 'Lê Văn Giang', '0955208647', '24/05/1998', 'Đà Nẵng', 'Đang học', 'Nam', 'le.van.giang@gmail.com', '944557669590', 'img/portrait/default.jpg'),
(66, 1, 4, 'Huỳnh Văn Bình', '0961616561', '01/02/1995', 'Huế', 'Đang học', 'Nam', 'huynh.van.binh@gmail.com', '799570235139', 'img/portrait/default.jpg'),
(67, 2, 4, 'Ngô Văn Giang', '0994695374', '24/06/1999', 'Lạng Sơn', 'Đang học', 'Nam', 'ngo.van.giang@gmail.com', '696426549168', 'img/portrait/default.jpg'),
(68, 1, 4, 'Đặng Thị Cúc', '0925744583', '22/10/1994', 'Lạng Sơn', 'Đang học', 'Nữ', 'dang.thi.cuc@gmail.com', '074688902565', 'img/portrait/default.jpg'),
(69, 1, 4, 'Ngô Thị Lan', '0992555405', '15/03/1991', 'Huế', 'Đang học', 'Nữ', 'ngo.thi.lan@gmail.com', '720559664188', 'img/portrait/default.jpg'),
(70, 10, 4, 'Hoàng Văn Anh', '0943706812', '22/02/1994', 'Quảng Ngãi', 'Đang học', 'Nam', 'hoang.van.anh@gmail.com', '319549557863', 'img/portrait/default.jpg'),
(71, 4, 4, 'Hồ Văn Minh', '0953744269', '09/08/2003', 'Đà Nẵng', 'Đang học', 'Nam', 'ho.van.minh@gmail.com', '148679494738', 'img/portrait/default.jpg'),
(72, 7, 4, 'Hồ Văn Đức', '0938058555', '16/01/1990', 'Hải Phòng', 'Đang học', 'Nam', 'ho.van.duc@gmail.com', '895088328175', 'img/portrait/default.jpg'),
(73, 5, 4, 'Hồ Thị Mai', '0929531543', '10/04/1995', 'Bắc Ninh', 'Đang học', 'Nữ', 'ho.thi.mai@gmail.com', '513870546906', 'img/portrait/default.jpg'),
(74, 2, 4, 'Nguyễn Văn Cường', '0976547685', '15/06/2001', 'Bắc Ninh', 'Đang học', 'Nam', 'nguyen.van.cuong@gmail.com', '053170961662', 'img/portrait/default.jpg'),
(75, 8, 4, 'Hoàng Văn Cường', '0999611726', '24/04/1997', 'Hồ Chí Minh', 'Đang học', 'Nam', 'hoang.van.cuong@gmail.com', '003559424992', 'img/portrait/default.jpg'),
(76, 4, 4, 'Huỳnh Văn Bình', '0967573183', '11/04/1998', 'Đà Nẵng', 'Đang học', 'Nam', 'huynh.van.binh@gmail.com', '479932826903', 'img/portrait/default.jpg'),
(77, 1, 4, 'Huỳnh Thị Hoa', '0961158550', '23/06/1999', 'Bình Định', 'Đang học', 'Nữ', 'huynh.thi.hoa@gmail.com', '837219077636', 'img/portrait/default.jpg'),
(78, 5, 4, 'Ngô Thị Mỹ', '0936521110', '08/07/1995', 'Cần Thơ', 'Đang học', 'Nữ', 'ngo.thi.my@gmail.com', '374116089691', 'img/portrait/default.jpg'),
(79, 8, 4, 'Hồ Thị Thu', '0926189119', '04/03/1993', 'Đà Nẵng', 'Đang học', 'Nữ', 'ho.thi.thu@gmail.com', '961452555996', 'img/portrait/default.jpg'),
(80, 8, 4, 'Đặng Văn Hải', '0941564589', '19/10/1990', 'Đà Nẵng', 'Đang học', 'Nam', 'dang.van.hai@gmail.com', '154576379819', 'img/portrait/default.jpg'),
(81, 2, 4, 'Lý Văn Bình', '0914568334', '10/05/1993', 'Bình Định', 'Đang học', 'Nam', 'ly.van.binh@gmail.com', '163670131517', 'img/portrait/default.jpg'),
(82, 5, 4, 'Hồ Văn Huy', '0953934716', '06/09/1995', 'Thái Nguyên', 'Đang học', 'Nam', 'ho.van.huy@gmail.com', '993768649677', 'img/portrait/default.jpg'),
(83, 6, 4, 'Trần Thị Cúc', '0920175687', '13/06/2002', 'Lạng Sơn', 'Đang học', 'Nữ', 'tran.thi.cuc@gmail.com', '018738005379', 'img/portrait/default.jpg'),
(84, 9, 5, 'Phạm Thị Bình', '0922552462', '10/11/2000', 'Quảng Ninh', 'Đang học', 'Nữ', 'pham.thi.binh@gmail.com', '881680957734', 'img/portrait/default.jpg'),
(85, 7, 5, 'Võ Thị Oanh', '0986276236', '17/05/1995', 'Nha Trang', 'Đang học', 'Nữ', 'vo.thi.oanh@gmail.com', '920535857280', 'img/portrait/default.jpg'),
(86, 5, 5, 'Dương Văn Cường', '0917555451', '01/02/1997', 'Quảng Ngãi', 'Đang học', 'Nam', 'duong.van.cuong@gmail.com', '651917994714', 'img/portrait/default.jpg'),
(87, 2, 5, 'Ngô Thị Oanh', '0947950911', '01/08/2001', 'Hà Nội', 'Đang học', 'Nữ', 'ngo.thi.oanh@gmail.com', '835561995116', 'img/portrait/default.jpg'),
(88, 2, 5, 'Phạm Thị Linh', '0977952037', '16/11/1991', 'Huế', 'Đang học', 'Nữ', 'pham.thi.linh@gmail.com', '891403165604', 'img/portrait/default.jpg'),
(89, 5, 5, 'Lý Thị Bình', '0955900775', '07/09/1990', 'Quảng Ngãi', 'Đang học', 'Nữ', 'ly.thi.binh@gmail.com', '978006898232', 'img/portrait/default.jpg'),
(90, 8, 5, 'Nguyễn Thị Thu', '0972938415', '25/05/2003', 'Quảng Ninh', 'Đang học', 'Nữ', 'nguyen.thi.thu@gmail.com', '420024687708', 'img/portrait/default.jpg'),
(91, 10, 5, 'Lý Thị Bình', '0981172125', '17/01/2003', 'Quảng Ninh', 'Đang học', 'Nữ', 'ly.thi.binh@gmail.com', '128252962386', 'img/portrait/default.jpg'),
(92, 4, 5, 'Hoàng Văn Anh', '0983156972', '01/04/1996', 'Quảng Ngãi', 'Đang học', 'Nam', 'hoang.van.anh@gmail.com', '172741896644', 'img/portrait/default.jpg'),
(93, 6, 5, 'Bùi Văn Đạt', '0983257300', '17/11/2002', 'Quảng Ngãi', 'Đang học', 'Nam', 'bui.van.dat@gmail.com', '113644139259', 'img/portrait/default.jpg'),
(94, 7, 5, 'Phạm Thị Hương', '0923239510', '06/12/1990', 'Hồ Chí Minh', 'Đang học', 'Nữ', 'pham.thi.huong@gmail.com', '731773552699', 'img/portrait/default.jpg'),
(95, 5, 5, 'Bùi Văn Khang', '0974709599', '27/12/2005', 'Phú Yên', 'Đang học', 'Nam', 'bui.van.khang@gmail.com', '583730575249', 'img/portrait/default.jpg'),
(96, 7, 5, 'Hồ Thị Linh', '0955422008', '08/09/2000', 'Bình Định', 'Đang học', 'Nữ', 'ho.thi.linh@gmail.com', '390012645510', 'img/portrait/default.jpg'),
(97, 8, 5, 'Trần Thị Trang', '0982540897', '18/10/1999', 'Bắc Ninh', 'Đang học', 'Nữ', 'tran.thi.trang@gmail.com', '475594390616', 'img/portrait/default.jpg'),
(98, 4, 5, 'Lý Văn Đạt', '0940924200', '17/08/1996', 'Hải Phòng', 'Đang học', 'Nam', 'ly.van.dat@gmail.com', '837736155058', 'img/portrait/default.jpg'),
(99, 7, 5, 'Hồ Thị Bình', '0941698936', '25/09/1993', 'Đà Nẵng', 'Đang học', 'Nữ', 'ho.thi.binh@gmail.com', '588672583627', 'img/portrait/default.jpg'),
(100, 5, 5, 'Lý Thị Lan', '0930481636', '02/07/2002', 'Vũng Tàu', 'Đang học', 'Nữ', 'ly.thi.lan@gmail.com', '072531292777', 'img/portrait/default.jpg'),
(101, 10, 5, 'Dương Thị Ngọc', '0970096576', '17/06/1999', 'Phú Yên', 'Đang học', 'Nữ', 'duong.thi.ngoc@gmail.com', '617807046359', 'img/portrait/default.jpg'),
(102, 6, 5, 'Vũ Thị Cúc', '0988369020', '07/03/1998', 'Phú Yên', 'Đang học', 'Nữ', 'vu.thi.cuc@gmail.com', '996749806559', 'img/portrait/default.jpg'),
(103, 8, 5, 'Lý Thị Lan', '0952271441', '26/04/1992', 'Hải Phòng', 'Đang học', 'Nữ', 'ly.thi.lan@gmail.com', '009106398963', 'img/portrait/default.jpg');

-- 2
INSERT INTO Lop (MaGV, MaNganh, TenLop, SoLuongSV) VALUES
                                                       (1, 1, 'CNTT01', 50),
                                                       (2, 2, 'CNTT02', 45),
                                                       (3, 3, 'CNTT03', 60),
                                                       (4, 4, 'CNTT04', 55),
                                                       (5, 5, 'CNTT05', 40),

                                                       (6, 6, 'KT01', 65),
                                                       (7, 7, 'KT02', 50),
                                                       (8, 8, 'KT03', 45),
                                                       (9, 9, 'KT04', 60),
                                                       (10, 10, 'KT05', 55);

-- 3
INSERT INTO GiangVien
(MaTK, MaKhoa, TenGV, NgaySinhGV, GioiTinhGV, SoDienThoai, Email, AnhDaiDienGV) VALUES
(1, 1, 'Nguyễn Văn An', '15/03/1980', 'Nam', '0912345678', 'an.nguyen@univ.edu.vn', 'img/portrait/default.jpg'),
(4, 1, 'Nguyễn Ân GV', '22/07/1982', 'Nữ', '0923456789', 'nguyen.an.gv@univ.edu.vn', 'img/portrait/default.jpg'),
(2, 1, 'Lê Quang Huy', '10/05/1985', 'Nam', '0934567890', 'huy.le@univ.edu.vn', 'img/portrait/default.jpg'),
(2, 1, 'Phạm Minh Châu', '30/09/1983', 'Nữ', '0945678901', 'chau.pham@univ.edu.vn', 'img/portrait/default.jpg'),
(2, 1, 'Đỗ Thị Thu Hà', '25/01/1987', 'Nữ', '0956789012', 'ha.do@univ.edu.vn', 'img/portrait/default.jpg'),
(2, 2, 'Ngô Văn Dũng', '12/11/1979', 'Nam', '0967890123', 'dung.ngo@univ.edu.vn', 'img/portrait/default.jpg'),
(2, 2, 'Vũ Thị Mai', '18/04/1986', 'Nữ', '0978901234', 'mai.vu@univ.edu.vn', 'img/portrait/default.jpg'),
(2, 2, 'Bùi Anh Tuấn', '05/08/1984', 'Nam', '0989012345', 'tuan.bui@univ.edu.vn', 'img/portrait/default.jpg'),
(2, 2, 'Hoàng Lan Phương', '14/02/1988', 'Nữ', '0990123456', 'phuong.hoang@univ.edu.vn', 'img/portrait/default.jpg'),
(2, 2, 'Phan Văn Khánh', '20/12/1978', 'Nam', '0901234567', 'khanh.phan@univ.edu.vn', 'img/portrait/default.jpg');


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
-- Mỗi NHP có 5sv
INSERT INTO DangKy (MaNHP, MaSV) VALUES
(1, 21),
(1, 22),
(1, 23),
(1, 24),
(1, 25),
(2, 26),
(2, 27),
(2, 28),
(2, 29),
(2, 30),
(3, 31),
(3, 32),
(3, 33),
(3, 34),
(3, 35),
(4, 36),
(4, 37),
(4, 38),
(4, 39),
(4, 40),
(5, 41),
(5, 42),
(5, 43),
(5, 44),
(5, 45),
(6, 46),
(6, 47),
(6, 48),
(6, 49),
(6, 50),
(7, 51),
(7, 52),
(7, 53),
(7, 54),
(7, 55),
(8, 56),
(8, 57),
(8, 58),
(8, 59),
(8, 60),
(9, 61),
(9, 62),
(9, 63),
(9, 64),
(9, 65),
(10, 66),
(10, 67),
(10, 68),
(10, 69),
(10, 70),
(11, 71),
(11, 72),
(11, 73),
(11, 74),
(11, 75),
(12, 76),
(12, 77),
(12, 78),
(12, 79),
(12, 80),
(13, 81),
(13, 82),
(13, 83),
(13, 84),
(13, 85),
(14, 86),
(14, 87),
(14, 88),
(14, 89),
(14, 90),
(15, 91),
(15, 92),
(15, 93),
(15, 94),
(15, 95),
(16, 96),
(16, 97),
(16, 98),
(16, 99),
(16, 100);


-- 12
INSERT INTO NhomHocPhan (MaGV, MaHP, MaLichDK, MaLop, HocKy, Nam, SiSo) VALUES
(2, 1, 2, 1, 2, 2024, 50),
(2, 2, 3, 2, 1, 2025, 45),
(2, 3, 2, 3, 2, 2024, 55),
(2, 4, 3, 4, 1, 2025, 60),
(2, 5, 2, 5, 2, 2024, 40),
(2, 6, 3, 6, 1, 2025, 50),
(2, 7, 2, 7, 2, 2024, 48),
(2, 8, 3, 8, 1, 2025, 52),
(2, 9, 2, 9, 2, 2024, 47),
(2, 10, 3, 10, 1, 2025, 53),
(2, 1, 2, 2, 2, 2024, 60),
(2, 2, 3, 3, 1, 2025, 58),
(2, 3, 2, 4, 2, 2024, 42),
(2, 4, 3, 5, 1, 2025, 50),
(2, 5, 2, 1, 2, 2024, 45),
(2, 6, 3, 6, 1, 2025, 45);



-- 13
INSERT INTO LichHoc (MaLH, MaPH, MaNHP, Thu, TietBatDau, TuNgay, DenNgay, TietKetThuc, SoTiet) VALUES
(1, 1, 1, 'Thứ 2', 1, '2025-02-17', '2025-06-01', 3, 3),
(2, 2, 2, 'Thứ 3', 4, '2025-02-17', '2025-06-01', 6, 3),
(3, 3, 3, 'Thứ 4', 1, '2025-02-17', '2025-06-01', 2, 2),
(4, 4, 4, 'Thứ 5', 7, '2025-02-17', '2025-06-01', 9, 3),
(5, 5, 5, 'Thứ 6', 1, '2025-02-17', '2025-06-01', 3, 3),
(6, 6, 6, 'Thứ 2', 4, '2025-02-17', '2025-06-01', 6, 3),
(7, 7, 7, 'Thứ 3', 7, '2025-02-17', '2025-06-01', 9, 3),
(8, 8, 8, 'Thứ 4', 1, '2025-02-17', '2025-06-01', 3, 3),
(9, 9, 9, 'Thứ 5', 4, '2025-02-17', '2025-06-01', 6, 3),
(10, 10, 10, 'Thứ 6', 7, '2025-02-17', '2025-06-01', 9, 3),
(11, 11, 11, 'Thứ 2', 1, '2025-02-17', '2025-06-01', 2, 2),
(12, 12, 12, 'Thứ 3', 4, '2025-02-17', '2025-06-01', 6, 3),
(13, 13, 13, 'Thứ 4', 7, '2025-02-17', '2025-06-01', 9, 3),
(14, 14, 14, 'Thứ 5', 1, '2025-02-17', '2025-06-01', 3, 3),
(15, 15, 15, 'Thứ 6', 4, '2025-02-17', '2025-06-01', 6, 3),
(16, 16, 16, 'Thứ 2', 7, '2025-02-17', '2025-06-01', 9, 3);


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



-- 17
INSERT INTO `KetQua` (`MaHP`, `MaSV`, `DiemThi`, `DiemHe4`, `DiemHe10`, `HocKy`, `Nam`) VALUES
(1, 21, 0.0, 0.0, 0.0, 1, '2025'),
(1, 22, 5.6, 2.0, 5.6, 1, '2025'),
(1, 23, 0.8, 0.0, 0.8, 1, '2025'),
(1, 24, 1.4, 0.0, 1.4, 1, '2025'),
(1, 25, 7.0, 3.0, 7.0, 1, '2025'),
(2, 26, 3.6, 0.0, 3.6, 1, '2025'),
(2, 27, 7.2, 3.0, 7.2, 1, '2025'),
(2, 28, 0.6, 0.0, 0.6, 1, '2025'),
(2, 29, 2.8, 0.0, 2.8, 1, '2025'),
(2, 30, 5.7, 2.0, 5.7, 1, '2025'),
(3, 31, 2.5, 0.0, 2.5, 1, '2025'),
(3, 32, 1.9, 0.0, 1.9, 1, '2025'),
(3, 33, 1.5, 0.0, 1.5, 1, '2025'),
(3, 34, 1.0, 0.0, 1.0, 1, '2025'),
(3, 35, 5.6, 2.0, 5.6, 1, '2025'),
(4, 36, 7.2, 3.0, 7.2, 1, '2025'),
(4, 37, 1.6, 0.0, 1.6, 1, '2025'),
(4, 38, 1.6, 0.0, 1.6, 1, '2025'),
(4, 39, 8.1, 3.0, 8.1, 1, '2025'),
(4, 40, 4.6, 1.0, 4.6, 1, '2025'),
(5, 41, 0.8, 0.0, 0.8, 1, '2025'),
(5, 42, 2.4, 0.0, 2.4, 1, '2025'),
(5, 43, 6.0, 2.0, 6.0, 1, '2025'),
(5, 44, 0.4, 0.0, 0.4, 1, '2025'),
(5, 45, 8.4, 3.0, 8.4, 1, '2025'),
(6, 46, 3.8, 0.0, 3.8, 1, '2025'),
(6, 47, 6.1, 2.0, 6.1, 1, '2025'),
(6, 48, 8.7, 4.0, 8.7, 1, '2025'),
(6, 49, 0.5, 0.0, 0.5, 1, '2025'),
(6, 50, 8.6, 4.0, 8.6, 1, '2025'),
(7, 51, 0.5, 0.0, 0.5, 1, '2025'),
(7, 52, 4.6, 1.0, 4.6, 1, '2025'),
(7, 53, 5.8, 2.0, 5.8, 1, '2025'),
(7, 54, 6.0, 2.0, 6.0, 1, '2025'),
(7, 55, 3.7, 0.0, 3.7, 1, '2025'),
(8, 56, 9.0, 4.0, 9.0, 1, '2025'),
(8, 57, 6.0, 2.0, 6.0, 1, '2025'),
(8, 58, 1.3, 0.0, 1.3, 1, '2025'),
(8, 59, 8.4, 3.0, 8.4, 1, '2025'),
(8, 60, 2.3, 0.0, 2.3, 1, '2025'),
(9, 61, 2.9, 0.0, 2.9, 1, '2025'),
(9, 62, 7.6, 3.0, 7.6, 1, '2025'),
(9, 63, 8.6, 4.0, 8.6, 1, '2025'),
(9, 64, 10.0, 4.0, 10.0, 1, '2025'),
(9, 65, 4.9, 1.0, 4.9, 1, '2025'),
(10, 66, 7.6, 3.0, 7.6, 1, '2025'),
(10, 67, 1.9, 0.0, 1.9, 1, '2025'),
(10, 68, 7.5, 3.0, 7.5, 1, '2025'),
(10, 69, 7.6, 3.0, 7.6, 1, '2025'),
(10, 70, 4.5, 1.0, 4.5, 1, '2025'),
(1, 71, 8.1, 3.0, 8.1, 1, '2025'),
(1, 72, 4.9, 1.0, 4.9, 1, '2025'),
(1, 73, 5.6, 2.0, 5.6, 1, '2025'),
(1, 74, 5.1, 1.0, 5.1, 1, '2025'),
(1, 75, 3.6, 0.0, 3.6, 1, '2025'),
(2, 76, 8.9, 4.0, 8.9, 1, '2025'),
(2, 77, 1.8, 0.0, 1.8, 1, '2025'),
(2, 78, 6.2, 2.0, 6.2, 1, '2025'),
(2, 79, 7.5, 3.0, 7.5, 1, '2025'),
(2, 80, 6.2, 2.0, 6.2, 1, '2025'),
(3, 81, 9.2, 4.0, 9.2, 1, '2025'),
(3, 82, 5.0, 1.0, 5.0, 1, '2025'),
(3, 83, 4.5, 1.0, 4.5, 1, '2025'),
(3, 84, 2.2, 0.0, 2.2, 1, '2025'),
(3, 85, 9.0, 4.0, 9.0, 1, '2025'),
(4, 86, 1.5, 0.0, 1.5, 1, '2025'),
(4, 87, 1.4, 0.0, 1.4, 1, '2025'),
(4, 88, 6.2, 2.0, 6.2, 1, '2025'),
(4, 89, 6.3, 2.0, 6.3, 1, '2025'),
(4, 90, 9.1, 4.0, 9.1, 1, '2025'),
(5, 91, 2.0, 0.0, 2.0, 1, '2025'),
(5, 92, 0.7, 0.0, 0.7, 1, '2025'),
(5, 93, 9.7, 4.0, 9.7, 1, '2025'),
(5, 94, 6.1, 2.0, 6.1, 1, '2025'),
(5, 95, 5.5, 2.0, 5.5, 1, '2025'),
(6, 96, 6.4, 2.0, 6.4, 1, '2025'),
(6, 97, 9.5, 4.0, 9.5, 1, '2025'),
(6, 98, 9.4, 4.0, 9.4, 1, '2025'),
(6, 99, 2.6, 0.0, 2.6, 1, '2025'),
(6, 100, 1.3, 0.0, 1.3, 1, '2025');


-- 19
INSERT INTO `DiemQuaTrinh` (`MaKQ`, `DiemSo`, `Status`) VALUES
(1, 8.5, 1),
(2, 7.2, 1),
(3, 6.0, 1),
(4, 9.1, 1),
(5, 4.3, 1),
(6, 5.7, 1),
(7, 8.9, 1),
(8, 3.4, 1),
(9, 7.8, 1),
(10, 6.5, 1),
(11, 9.0, 1),
(12, 2.1, 1),
(13, 8.2, 1),
(14, 5.9, 1),
(15, 7.4, 1),
(16, 4.8, 1),
(17, 6.7, 1),
(18, 9.3, 1),
(19, 3.6, 1),
(20, 8.0, 1),
(21, 7.1, 1),
(22, 5.4, 1),
(23, 9.5, 1),
(24, 4.2, 1),
(25, 6.3, 1),
(26, 8.7, 1),
(27, 3.9, 1),
(28, 7.6, 1),
(29, 5.8, 1),
(30, 9.2, 1),
(31, 4.5, 1),
(32, 6.9, 1),
(33, 8.4, 1),
(34, 3.2, 1),
(35, 7.3, 1),
(36, 5.5, 1),
(37, 9.4, 1),
(38, 4.1, 1),
(39, 6.6, 1),
(40, 8.8, 1),
(41, 3.7, 1),
(42, 7.7, 1),
(43, 5.6, 1),
(44, 9.6, 1),
(45, 4.4, 1),
(46, 6.4, 1),
(47, 8.6, 1),
(48, 3.5, 1),
(49, 7.5, 1),
(50, 5.3, 1),
(51, 9.7, 1),
(52, 4.0, 1),
(53, 6.8, 1),
(54, 8.3, 1),
(55, 3.3, 1),
(56, 7.0, 1),
(57, 5.2, 1),
(58, 9.8, 1),
(59, 4.6, 1),
(60, 6.2, 1),
(61, 8.1, 1),
(62, 3.8, 1),
(63, 7.9, 1),
(64, 5.1, 1),
(65, 9.9, 1),
(66, 4.7, 1),
(67, 6.1, 1),
(68, 8.0, 1),
(69, 3.1, 1),
(70, 7.2, 1),
(71, 5.0, 1),
(72, 10.0, 1),
(73, 4.9, 1),
(74, 6.0, 1),
(75, 7.8, 1),
(76, 3.0, 1),
(77, 8.5, 1),
(78, 5.5, 1),
(79, 9.0, 1),
(80, 4.8, 1);



-- 20
INSERT INTO `CotDiem` (`MaDQT`, `TenCotDiem`, `DiemSo`, `HeSo`, `Status`) VALUES
(1, 'Cột điểm 1', 8.5, 1, 1),
(2, 'Cột điểm 1', 7.2, 1, 1),
(3, 'Cột điểm 1', 6.0, 1, 1),
(4, 'Cột điểm 1', 9.1, 1, 1),
(5, 'Cột điểm 1', 4.3, 1, 1),
(6, 'Cột điểm 1', 5.7, 1, 1),
(7, 'Cột điểm 1', 8.9, 1, 1),
(8, 'Cột điểm 1', 3.4, 1, 1),
(9, 'Cột điểm 1', 7.8, 1, 1),
(10, 'Cột điểm 1', 6.5, 1, 1),
(11, 'Cột điểm 1', 9.0, 1, 1),
(12, 'Cột điểm 1', 2.1, 1, 1),
(13, 'Cột điểm 1', 8.2, 1, 1),
(14, 'Cột điểm 1', 5.9, 1, 1),
(15, 'Cột điểm 1', 7.4, 1, 1),
(16, 'Cột điểm 1', 4.8, 1, 1),
(17, 'Cột điểm 1', 6.7, 1, 1),
(18, 'Cột điểm 1', 9.3, 1, 1),
(19, 'Cột điểm 1', 3.6, 1, 1),
(20, 'Cột điểm 1', 8.0, 1, 1),
(21, 'Cột điểm 1', 7.1, 1, 1),
(22, 'Cột điểm 1', 5.4, 1, 1),
(23, 'Cột điểm 1', 9.5, 1, 1),
(24, 'Cột điểm 1', 4.2, 1, 1),
(25, 'Cột điểm 1', 6.3, 1, 1),
(26, 'Cột điểm 1', 8.7, 1, 1),
(27, 'Cột điểm 1', 3.9, 1, 1),
(28, 'Cột điểm 1', 7.6, 1, 1),
(29, 'Cột điểm 1', 5.8, 1, 1),
(30, 'Cột điểm 1', 9.2, 1, 1),
(31, 'Cột điểm 1', 4.5, 1, 1),
(32, 'Cột điểm 1', 6.9, 1, 1),
(33, 'Cột điểm 1', 8.4, 1, 1),
(34, 'Cột điểm 1', 3.2, 1, 1),
(35, 'Cột điểm 1', 7.3, 1, 1),
(36, 'Cột điểm 1', 5.5, 1, 1),
(37, 'Cột điểm 1', 9.4, 1, 1),
(38, 'Cột điểm 1', 4.1, 1, 1),
(39, 'Cột điểm 1', 6.6, 1, 1),
(40, 'Cột điểm 1', 8.8, 1, 1),
(41, 'Cột điểm 1', 3.7, 1, 1),
(42, 'Cột điểm 1', 7.7, 1, 1),
(43, 'Cột điểm 1', 5.6, 1, 1),
(44, 'Cột điểm 1', 9.6, 1, 1),
(45, 'Cột điểm 1', 4.4, 1, 1),
(46, 'Cột điểm 1', 6.4, 1, 1),
(47, 'Cột điểm 1', 8.6, 1, 1),
(48, 'Cột điểm 1', 3.5, 1, 1),
(49, 'Cột điểm 1', 7.5, 1, 1),
(50, 'Cột điểm 1', 5.3, 1, 1),
(51, 'Cột điểm 1', 9.7, 1, 1),
(52, 'Cột điểm 1', 4.0, 1, 1),
(53, 'Cột điểm 1', 6.8, 1, 1),
(54, 'Cột điểm 1', 8.3, 1, 1),
(55, 'Cột điểm 1', 3.3, 1, 1),
(56, 'Cột điểm 1', 7.0, 1, 1),
(57, 'Cột điểm 1', 5.2, 1, 1),
(58, 'Cột điểm 1', 9.8, 1, 1),
(59, 'Cột điểm 1', 4.6, 1, 1),
(60, 'Cột điểm 1', 6.2, 1, 1),
(61, 'Cột điểm 1', 8.1, 1, 1),
(62, 'Cột điểm 1', 3.8, 1, 1),
(63, 'Cột điểm 1', 7.9, 1, 1),
(64, 'Cột điểm 1', 5.1, 1, 1),
(65, 'Cột điểm 1', 9.9, 1, 1),
(66, 'Cột điểm 1', 4.7, 1, 1),
(67, 'Cột điểm 1', 6.1, 1, 1),
(68, 'Cột điểm 1', 8.0, 1, 1),
(69, 'Cột điểm 1', 3.1, 1, 1),
(70, 'Cột điểm 1', 7.2, 1, 1),
(71, 'Cột điểm 1', 5.0, 1, 1),
(72, 'Cột điểm 1', 10.0, 1, 1),
(73, 'Cột điểm 1', 4.9, 1, 1),
(74, 'Cột điểm 1', 6.0, 1, 1),
(75, 'Cột điểm 1', 7.8, 1, 1),
(76, 'Cột điểm 1', 3.0, 1, 1),
(77, 'Cột điểm 1', 8.5, 1, 1),
(78, 'Cột điểm 1', 5.5, 1, 1),
(79, 'Cột điểm 1', 9.0, 1, 1),
(80, 'Cột điểm 1', 4.8, 1, 1);

INSERT INTO `CotDiem` (`MaDQT`, `TenCotDiem`, `DiemSo`, `HeSo`, `Status`) VALUES
(1, 'Cột điểm 2', 8.5, 1, 1),
(2, 'Cột điểm 2', 7.2, 1, 1),
(3, 'Cột điểm 2', 6.0, 1, 1),
(4, 'Cột điểm 2', 9.1, 1, 1),
(5, 'Cột điểm 2', 4.3, 1, 1),
(6, 'Cột điểm 2', 5.7, 1, 1),
(7, 'Cột điểm 2', 8.9, 1, 1),
(8, 'Cột điểm 2', 3.4, 1, 1),
(9, 'Cột điểm 2', 7.8, 1, 1),
(10, 'Cột điểm 2', 6.5, 1, 1),
(11, 'Cột điểm 2', 9.0, 1, 1),
(12, 'Cột điểm 2', 2.1, 1, 1),
(13, 'Cột điểm 2', 8.2, 1, 1),
(14, 'Cột điểm 2', 5.9, 1, 1),
(15, 'Cột điểm 2', 7.4, 1, 1),
(16, 'Cột điểm 2', 4.8, 1, 1),
(17, 'Cột điểm 2', 6.7, 1, 1),
(18, 'Cột điểm 2', 9.3, 1, 1),
(19, 'Cột điểm 2', 3.6, 1, 1),
(20, 'Cột điểm 2', 8.0, 1, 1),
(21, 'Cột điểm 2', 7.1, 1, 1),
(22, 'Cột điểm 2', 5.4, 1, 1),
(23, 'Cột điểm 2', 9.5, 1, 1),
(24, 'Cột điểm 2', 4.2, 1, 1),
(25, 'Cột điểm 2', 6.3, 1, 1),
(26, 'Cột điểm 2', 8.7, 1, 1),
(27, 'Cột điểm 2', 3.9, 1, 1),
(28, 'Cột điểm 2', 7.6, 1, 1),
(29, 'Cột điểm 2', 5.8, 1, 1),
(30, 'Cột điểm 2', 9.2, 1, 1),
(31, 'Cột điểm 2', 4.5, 1, 1),
(32, 'Cột điểm 2', 6.9, 1, 1),
(33, 'Cột điểm 2', 8.4, 1, 1),
(34, 'Cột điểm 2', 3.2, 1, 1),
(35, 'Cột điểm 2', 7.3, 1, 1),
(36, 'Cột điểm 2', 5.5, 1, 1),
(37, 'Cột điểm 2', 9.4, 1, 1),
(38, 'Cột điểm 2', 4.1, 1, 1),
(39, 'Cột điểm 2', 6.6, 1, 1),
(40, 'Cột điểm 2', 8.8, 1, 1),
(41, 'Cột điểm 2', 3.7, 1, 1),
(42, 'Cột điểm 2', 7.7, 1, 1),
(43, 'Cột điểm 2', 5.6, 1, 1),
(44, 'Cột điểm 2', 9.6, 1, 1),
(45, 'Cột điểm 2', 4.4, 1, 1),
(46, 'Cột điểm 2', 6.4, 1, 1),
(47, 'Cột điểm 2', 8.6, 1, 1),
(48, 'Cột điểm 2', 3.5, 1, 1),
(49, 'Cột điểm 2', 7.5, 1, 1),
(50, 'Cột điểm 2', 5.3, 1, 1),
(51, 'Cột điểm 2', 9.7, 1, 1),
(52, 'Cột điểm 2', 4.0, 1, 1),
(53, 'Cột điểm 2', 6.8, 1, 1),
(54, 'Cột điểm 2', 8.3, 1, 1),
(55, 'Cột điểm 2', 3.3, 1, 1),
(56, 'Cột điểm 2', 7.0, 1, 1),
(57, 'Cột điểm 2', 5.2, 1, 1),
(58, 'Cột điểm 2', 9.8, 1, 1),
(59, 'Cột điểm 2', 4.6, 1, 1),
(60, 'Cột điểm 2', 6.2, 1, 1),
(61, 'Cột điểm 2', 8.1, 1, 1),
(62, 'Cột điểm 2', 3.8, 1, 1),
(63, 'Cột điểm 2', 7.9, 1, 1),
(64, 'Cột điểm 2', 5.1, 1, 1),
(65, 'Cột điểm 2', 9.9, 1, 1),
(66, 'Cột điểm 2', 4.7, 1, 1),
(67, 'Cột điểm 2', 6.1, 1, 1),
(68, 'Cột điểm 2', 8.0, 1, 1),
(69, 'Cột điểm 2', 3.1, 1, 1),
(70, 'Cột điểm 2', 7.2, 1, 1),
(71, 'Cột điểm 2', 5.0, 1, 1),
(72, 'Cột điểm 2', 10.0, 1, 1),
(73, 'Cột điểm 2', 4.9, 1, 1),
(74, 'Cột điểm 2', 6.0, 1, 1),
(75, 'Cột điểm 2', 7.8, 1, 1),
(76, 'Cột điểm 2', 3.0, 1, 1),
(77, 'Cột điểm 2', 8.5, 1, 1),
(78, 'Cột điểm 2', 5.5, 1, 1),
(79, 'Cột điểm 2', 9.0, 1, 1),
(80, 'Cột điểm 2', 4.8, 1, 1);





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
INSERT INTO `CaThi` (`MaHP`, `MaPH`, `Thu`, `ThoiGianBatDau`, `ThoiLuong`, `HocKy`, `Nam`, `Status`)
VALUES
-- 5 ca thi đầu: Học kỳ 2, năm 2024
(1, 1, 'Thứ 2', '15/05/2024 07:30:00', '01:30', 2, '2024', 1),
(2, 2, 'Thứ 3', '16/05/2024 09:00:00', '01:30', 2, '2024', 1),
(3, 3, 'Thứ 4', '17/05/2024 13:00:00', '02:00', 2, '2024', 1),
(4, 4, 'Thứ 5', '18/05/2024 15:00:00', '01:45', 2, '2024', 1),
(5, 5, 'Thứ 6', '19/05/2024 08:00:00', '01:30', 2, '2024', 1),

-- 5 ca thi sau: Học kỳ 1, năm 2025
(6, 6, 'Thứ 2', '10/10/2025 07:30:00', '01:30', 1, '2025', 1),
(7, 7, 'Thứ 3', '11/10/2025 09:00:00', '01:30', 1, '2025', 1),
(8, 8, 'Thứ 4', '12/10/2025 13:00:00', '02:00', 1, '2025', 1),
(9, 9, 'Thứ 5', '13/10/2025 15:00:00', '01:45', 1, '2025', 1),
(10, 10, 'Thứ 6', '14/10/2025 08:00:00', '01:30', 1, '2025', 1);





-- 23
INSERT INTO `CaThi_SinhVien` (`MaCT`, `MaSV`, `Status`)
VALUES
-- Ca thi 1: Sinh viên 1-5
(1, 1, 1),
(1, 2, 1),
(1, 3, 1),
(1, 4, 1),
(1, 5, 1),

-- Ca thi 2: Sinh viên 6-10
(2, 6, 1),
(2, 7, 1),
(2, 8, 1),
(2, 9, 1),
(2, 10, 1),

-- Ca thi 3: Sinh viên 11-15
(3, 11, 1),
(3, 12, 1),
(3, 13, 1),
(3, 14, 1),
(3, 15, 1),

-- Ca thi 4: Sinh viên 16-20
(4, 16, 1),
(4, 17, 1),
(4, 18, 1),
(4, 19, 1),
(4, 20, 1),

-- Ca thi 5: Sinh viên 21-25
(5, 21, 1),
(5, 22, 1),
(5, 23, 1),
(5, 24, 1),
(5, 25, 1),

-- Ca thi 6: Sinh viên 26-30
(6, 26, 1),
(6, 27, 1),
(6, 28, 1),
(6, 29, 1),
(6, 30, 1),

-- Ca thi 7: Sinh viên 31-35
(7, 31, 1),
(7, 32, 1),
(7, 33, 1),
(7, 34, 1),
(7, 35, 1),

-- Ca thi 8: Sinh viên 36-40
(8, 36, 1),
(8, 37, 1),
(8, 38, 1),
(8, 39, 1),
(8, 40, 1),

-- Ca thi 9: Sinh viên 41-45
(9, 41, 1),
(9, 42, 1),
(9, 43, 1),
(9, 44, 1),
(9, 45, 1),

-- Ca thi 10: Sinh viên 46-50
(10, 46, 1),
(10, 47, 1),
(10, 48, 1),
(10, 49, 1),
(10, 50, 1);


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
INSERT INTO ChuongTrinhDaoTao (MaCKDT, MaNganh, LoaiHinhDT, TrinhDo) VALUES
                                                                         (1, 1, 'Chính quy', 'Cử nhân'),
                                                                         (1, 2, 'Chính quy', 'Cử nhân'),
                                                                         (2, 1, 'Chính quy', 'Cử nhân'),
                                                                         (2, 2, 'Chính quy', 'Cử nhân');


-- 26
-- tài khoản quản trị viên
INSERT INTO TaiKhoan (MaNQ, TenDangNhap, MatKhau) VALUES
                                                      (2, 'admin', '$2a$11$MVrjcR5/HRmUmTiWNV2uX.6uGJyg/hwl.d9Mcr.g.M5N133J0Timu'),
                                                      (3, 'an', '$2a$11$YZQfbakMQk1uUZ0ojbU.gOg5zrd7ncbMcz4402rhq4nc4PjDYrtMK');
-- tài khoản sinh viên
INSERT INTO TaiKhoan (MaNQ, TenDangNhap, MatKhau, Type) VALUES
						      (1, 'sinhvien', '$2a$11$YZQfbakMQk1uUZ0ojbU.gOg5zrd7ncbMcz4402rhq4nc4PjDYrtMK', 'Sinh viên');

-- tài khoản test giảng viên
INSERT INTO TaiKhoan (MaNQ, TenDangNhap, MatKhau) VALUES
						      (4, 'giangvien', '$2a$11$YZQfbakMQk1uUZ0ojbU.gOg5zrd7ncbMcz4402rhq4nc4PjDYrtMK');

-- 27
INSERT INTO NhomQuyen (TenNhomQuyen) VALUES
										('SinhVien'),
                                         ('Admin'),
                                         ('AnQuyen'),
										 ('GiangVien');

-- 28
INSERT INTO ChiTietQuyen (MaCN, MaNQ, HanhDong) VALUES
(1, 2, 'Xem'), (1, 2, 'Them'), (1, 2, 'Sua'), (1, 2, 'Xoa'),
(2, 2, 'Xem'), (2, 2, 'Them'), (2, 2, 'Sua'), (2, 2, 'Xoa'),
(3, 2, 'Xem'), (3, 2, 'Them'), (3, 2, 'Sua'), (3, 2, 'Xoa'),
(4, 2, 'Xem'), (4, 2, 'Them'), (4, 2, 'Sua'), (4, 2, 'Xoa'),
(5, 2, 'Xem'), (5, 2, 'Them'), (5, 2, 'Sua'), (5, 2, 'Xoa'),
(6, 2, 'Xem'), (6, 2, 'Them'), (6, 2, 'Sua'), (6, 2, 'Xoa'),
(7, 2, 'Xem'), (7, 2, 'Them'), (7, 2, 'Sua'), (7, 2, 'Xoa'),
(8, 2, 'Xem'), (8, 2, 'Them'), (8, 2, 'Sua'), (8, 2, 'Xoa'),
(9, 2, 'Xem'), (9, 2, 'Them'), (9, 2, 'Sua'), (9, 2, 'Xoa'),
(10, 2, 'Xem'), (10, 2, 'Them'), (10, 2, 'Sua'), (10, 2, 'Xoa'),
(11, 2, 'Xem'), (11, 2, 'Them'),
(12, 2, 'Xem'), (12, 2, 'Them'),
(13, 2, 'Xem'), (13, 2, 'Them'),
(14, 2, 'Xem'), (14, 2, 'Them'), (14, 2, 'Sua'), (14, 2, 'Xoa'),
(15, 2, 'Xem'), (15, 2, 'Them'), (15, 2, 'Sua'), (15, 2, 'Xoa'),
(16, 2, 'Xem'), (16, 2, 'Them'), (16, 2, 'Sua'), (16, 2, 'Xoa'),
(17, 2, 'Xem'),


(1, 3, 'Xem'), (1, 3, 'Them'), (1, 3, 'Sua'), (1, 3, 'Xoa'),
(2, 3, 'Xem'), (2, 3, 'Them'), (2, 3, 'Sua'), (2, 3, 'Xoa'),

(12, 4, 'Xem'), (12, 4, 'Them');

-- 29
INSERT INTO ChucNang (TenChucNang) VALUES
                                       ('SINHVIEN'),
                                       ('GIANGVIEN'),
                                       ('KHOA'),
                                       ('NGANH'),
					('LOP'),
                                       ('CHUONGTRINHDAOTAO'),
                                       ('HOCPHAN'),
                                       ('PHONGHOC'),
                                       ('CHUKYDAOTAO'),
                                       ('KHOAHOC'),
                                       ('TOCHUCTHI'),
                                       ('NHAPDIEM'),
                                       ('HOCPHI'),
                                       ('MODANGKYHOCPHAN'),
                                       ('TAIKHOAN'),
                                       ('PHANQUYEN'),
                                       ('THONGKE');

-- 30
INSERT INTO LichDangKy (ThoiGianBatDau, ThoiGianKetThuc, Status)
VALUES
('1990-01-01 08:00:00', '1990-01-03 08:00:00', 1),  -- Lịch ở năm 1990
('2024-12-15 10:00:00', '2024-12-17 10:00:00', 1),  -- Lịch cuối năm 2024
('2025-06-05 09:00:00', '2025-06-07 09:00:00', 1);  -- Lịch giữa năm 2025


-- TKSV

INSERT INTO `TaiKhoan` (`MaNQ`, `TenDangNhap`, `MatKhau`, `Type`) VALUES
(1, 'sv1', '123456', 'Sinh viên'),
(1, 'sv2', '123456', 'Sinh viên'),
(1, 'sv3', '123456', 'Sinh viên'),
(1, 'sv4', '123456', 'Sinh viên'),
(1, 'sv5', '123456', 'Sinh viên'),
(1, 'sv6', '123456', 'Sinh viên'),
(1, 'sv7', '123456', 'Sinh viên'),
(1, 'sv8', '123456', 'Sinh viên'),
(1, 'sv9', '123456', 'Sinh viên'),
(1, 'sv10', '123456', 'Sinh viên'),
(1, 'sv11', '123456', 'Sinh viên'),
(1, 'sv12', '123456', 'Sinh viên'),
(1, 'sv13', '123456', 'Sinh viên'),
(1, 'sv14', '123456', 'Sinh viên'),
(1, 'sv15', '123456', 'Sinh viên'),
(1, 'sv16', '123456', 'Sinh viên'),
(1, 'sv17', '123456', 'Sinh viên'),
(1, 'sv18', '123456', 'Sinh viên'),
(1, 'sv19', '123456', 'Sinh viên'),
(1, 'sv20', '123456', 'Sinh viên'),
(1, 'sv21', '123456', 'Sinh viên'),
(1, 'sv22', '123456', 'Sinh viên'),
(1, 'sv23', '123456', 'Sinh viên'),
(1, 'sv24', '123456', 'Sinh viên'),
(1, 'sv25', '123456', 'Sinh viên'),
(1, 'sv26', '123456', 'Sinh viên'),
(1, 'sv27', '123456', 'Sinh viên'),
(1, 'sv28', '123456', 'Sinh viên'),
(1, 'sv29', '123456', 'Sinh viên'),
(1, 'sv30', '123456', 'Sinh viên'),
(1, 'sv31', '123456', 'Sinh viên'),
(1, 'sv32', '123456', 'Sinh viên'),
(1, 'sv33', '123456', 'Sinh viên'),
(1, 'sv34', '123456', 'Sinh viên'),
(1, 'sv35', '123456', 'Sinh viên'),
(1, 'sv36', '123456', 'Sinh viên'),
(1, 'sv37', '123456', 'Sinh viên'),
(1, 'sv38', '123456', 'Sinh viên'),
(1, 'sv39', '123456', 'Sinh viên'),
(1, 'sv40', '123456', 'Sinh viên'),
(1, 'sv41', '123456', 'Sinh viên'),
(1, 'sv42', '123456', 'Sinh viên'),
(1, 'sv43', '123456', 'Sinh viên'),
(1, 'sv44', '123456', 'Sinh viên'),
(1, 'sv45', '123456', 'Sinh viên'),
(1, 'sv46', '123456', 'Sinh viên'),
(1, 'sv47', '123456', 'Sinh viên'),
(1, 'sv48', '123456', 'Sinh viên'),
(1, 'sv49', '123456', 'Sinh viên'),
(1, 'sv50', '123456', 'Sinh viên'),
(1, 'sv51', '123456', 'Sinh viên'),
(1, 'sv52', '123456', 'Sinh viên'),
(1, 'sv53', '123456', 'Sinh viên'),
(1, 'sv54', '123456', 'Sinh viên'),
(1, 'sv55', '123456', 'Sinh viên'),
(1, 'sv56', '123456', 'Sinh viên'),
(1, 'sv57', '123456', 'Sinh viên'),
(1, 'sv58', '123456', 'Sinh viên'),
(1, 'sv59', '123456', 'Sinh viên'),
(1, 'sv60', '123456', 'Sinh viên'),
(1, 'sv61', '123456', 'Sinh viên'),
(1, 'sv62', '123456', 'Sinh viên'),
(1, 'sv63', '123456', 'Sinh viên'),
(1, 'sv64', '123456', 'Sinh viên'),
(1, 'sv65', '123456', 'Sinh viên'),
(1, 'sv66', '123456', 'Sinh viên'),
(1, 'sv67', '123456', 'Sinh viên'),
(1, 'sv68', '123456', 'Sinh viên'),
(1, 'sv69', '123456', 'Sinh viên'),
(1, 'sv70', '123456', 'Sinh viên'),
(1, 'sv71', '123456', 'Sinh viên'),
(1, 'sv72', '123456', 'Sinh viên'),
(1, 'sv73', '123456', 'Sinh viên'),
(1, 'sv74', '123456', 'Sinh viên'),
(1, 'sv75', '123456', 'Sinh viên'),
(1, 'sv76', '123456', 'Sinh viên'),
(1, 'sv77', '123456', 'Sinh viên'),
(1, 'sv78', '123456', 'Sinh viên'),
(1, 'sv79', '123456', 'Sinh viên'),
(1, 'sv80', '123456', 'Sinh viên'),
(1, 'sv81', '123456', 'Sinh viên'),
(1, 'sv82', '123456', 'Sinh viên'),
(1, 'sv83', '123456', 'Sinh viên'),
(1, 'sv84', '123456', 'Sinh viên'),
(1, 'sv85', '123456', 'Sinh viên'),
(1, 'sv86', '123456', 'Sinh viên'),
(1, 'sv87', '123456', 'Sinh viên'),
(1, 'sv88', '123456', 'Sinh viên'),
(1, 'sv89', '123456', 'Sinh viên'),
(1, 'sv90', '123456', 'Sinh viên'),
(1, 'sv91', '123456', 'Sinh viên'),
(1, 'sv92', '123456', 'Sinh viên'),
(1, 'sv93', '123456', 'Sinh viên'),
(1, 'sv94', '123456', 'Sinh viên'),
(1, 'sv95', '123456', 'Sinh viên'),
(1, 'sv96', '123456', 'Sinh viên'),
(1, 'sv97', '123456', 'Sinh viên'),
(1, 'sv98', '123456', 'Sinh viên'),
(1, 'sv99', '123456', 'Sinh viên'),
(1, 'sv100', '123456', 'Sinh viên'),
(2, 'myadmin', '123456', 'Quản trị viên');



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
    ADD CONSTRAINT `LichHoc_NhomHocPhan` FOREIGN KEY (MaNHP) REFERENCES `NhomHocPhan`(MaNHP),
    ADD CONSTRAINT `LichHoc_PhongHoc` FOREIGN KEY (MaPH) REFERENCES `PhongHoc`(MaPH);


ALTER TABLE `NhomHocPhan`
    ADD CONSTRAINT `NhomHocPhan_HocPhan` FOREIGN KEY (MaHP) REFERENCES `HocPhan`(MaHP);

ALTER TABLE `CaThi`
    ADD CONSTRAINT `CaThi_PhongHoc` FOREIGN KEY (MaPH) REFERENCES `PhongHoc`(MaPH),
    ADD CONSTRAINT `CaThi_HocPhan` FOREIGN KEY (MaHP) REFERENCES `HocPhan`(MaHP);

ALTER TABLE `CaThi_SinhVien`
    ADD CONSTRAINT `CaThi_SinhVien_CaThi` FOREIGN KEY (MaCT) REFERENCES `CaThi`(MaCT),
    ADD CONSTRAINT `CaThi_SinhVien_SinhVien` FOREIGN KEY (MaSV) REFERENCES `SinhVien`(MaSV);




ALTER TABLE `HocPhiHocPhan`
    ADD CONSTRAINT `HocPhiHocPhan_HocPhan` FOREIGN KEY (MaHP) REFERENCES `HocPhan`(MaHP),
    ADD CONSTRAINT `HocPhiHocPhan_SinhVien` FOREIGN KEY (MaSV) REFERENCES `SinhVien`(MaSV);




ALTER TABLE `DiemQuaTrinh`
    ADD CONSTRAINT `DiemQuaTrinh_KetQua` FOREIGN KEY (MaKQ) REFERENCES `KetQua`(MaKQ);

ALTER TABLE `KetQua`
    ADD CONSTRAINT `KetQua_HocPhan` FOREIGN KEY (MaHP) REFERENCES `HocPhan`(MaHP);

ALTER TABLE `KetQua`
    ADD CONSTRAINT `KetQua_SinhVien` FOREIGN KEY (MaSV) REFERENCES `SinhVien`(MaSV);

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

ALTER TABLE `ChuongTrinhDaoTao_HocPhan`
    ADD CONSTRAINT `ChuongTrinhDaoTao_HocPhan_HocPhan` FOREIGN KEY (MaHP) REFERENCES `HocPhan`(MaHP),
    ADD CONSTRAINT `ChuongTrinhDaoTao_HocPhan_ChuongTrinhDaoTao` FOREIGN KEY (MaCTDT) REFERENCES `ChuongTrinhDaoTao`(MaCTDT);

ALTER TABLE `ChuongTrinhDaoTao`
    ADD CONSTRAINT `ChuongTrinhDaoTao_ChuKyDaoTao` FOREIGN KEY (MaCKDT) REFERENCES `ChuKyDaoTao`(MaCKDT),
    ADD CONSTRAINT `ChuongTrinhDaoTao_Nganh` FOREIGN KEY (MaNganh) REFERENCES `Nganh`(MaNganh);

ALTER TABLE `NhomHocPhan`
    ADD CONSTRAINT `NhomHocPhan_LichDangky` FOREIGN KEY (MaLichDK) REFERENCES `LichDangky`(MaLichDK);

ALTER TABLE `NhomHocPhan`
    ADD CONSTRAINT `NhomHocPhan_Lop` FOREIGN KEY (MaLop) REFERENCES `Lop`(MaLop);

SELECT * FROM SinhVien;
SELECT * FROM Nganh;
SELECT * FROM Lop;























