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
-- điểm thi chưa nhập
CREATE TABLE `KetQua` (
                          `MaKQ` INT AUTO_INCREMENT PRIMARY KEY,
                          `MaHP` INT,
                          `MaSV` INT,
                          `MaDQT` INT,
                          `DiemThi` float DEFAULT -1,
                          `DiemHe4` float DEFAULT 0,
                          `DiemHe10` float DEFAULT 0,
                          `HocKy` INT,
                          `Nam` VARCHAR(255),
                          Status TINYINT DEFAULT 1
);

-- 19
CREATE TABLE `DiemQuaTrinh` (
                                `MaDQT` INT AUTO_INCREMENT PRIMARY KEY,
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
INSERT INTO `SinhVien` (`MaTK`, `MaLop`, `MaKhoaHoc`, `TenSV`, `SoDienThoaiSV`, `NgaySinhSV`, `QueQuanSV`, `TrangThaiSV`, `GioiTinhSV`, `EmailSV`, `CCCDSV`, `AnhDaiDienSV`) VALUES
(4, 2, 1, 'Võ Thị Lan', '0968188598', '1995-06-24', 'Bình Định', 'Đang học', 'Nữ', 'vo.thi.lan@example.com', '528139535131', 'avatar_1.jpg'),
(5, 8, 1, 'Ngô Thị Thu', '0976545290', '1997-10-27', 'Vũng Tàu', 'Đang học', 'Nữ', 'ngo.thi.thu@example.com', '249229315668', 'avatar_2.jpg'),
(6, 4, 1, 'Trần Văn Anh', '0960877017', '1998-12-13', 'Huế', 'Đang học', 'Nam', 'tran.van.anh@example.com', '753662145005', 'avatar_3.jpg'),
(7, 6, 1, 'Dương Thị Lan', '0975025648', '2000-03-18', 'Nha Trang', 'Đang học', 'Nữ', 'duong.thi.lan@example.com', '254131630817', 'avatar_4.jpg'),
(8, 9, 1, 'Lý Thị Cúc', '0942677057', '2000-05-16', 'Phú Yên', 'Đang học', 'Nữ', 'ly.thi.cuc@example.com', '288784918817', 'avatar_5.jpg'),
(9, 9, 1, 'Võ Thị Nga', '0963229308', '1992-04-01', 'Hà Nội', 'Đang học', 'Nữ', 'vo.thi.nga@example.com', '139788401816', 'avatar_6.jpg'),
(10, 7, 1, 'Bùi Thị Lan', '0985119093', '2004-04-04', 'Cần Thơ', 'Đang học', 'Nữ', 'bui.thi.lan@example.com', '346597264307', 'avatar_7.jpg'),
(11, 10, 1, 'Bùi Thị Trang', '0975984367', '2001-07-07', 'Nha Trang', 'Đang học', 'Nữ', 'bui.thi.trang@example.com', '557048851125', 'avatar_8.jpg'),
(12, 2, 1, 'Bùi Văn Anh', '0938185337', '1990-10-23', 'Lạng Sơn', 'Đang học', 'Nam', 'bui.van.anh@example.com', '729358637684', 'avatar_9.jpg'),
(13, 5, 1, 'Dương Văn Bình', '0987631890', '2003-07-20', 'Phú Yên', 'Đang học', 'Nam', 'duong.van.binh@example.com', '952070995341', 'avatar_10.jpg'),
(14, 7, 1, 'Hoàng Văn Tuấn', '0954639950', '2002-09-02', 'Hải Phòng', 'Đang học', 'Nam', 'hoang.van.tuan@example.com', '728526660271', 'avatar_11.jpg'),
(15, 7, 1, 'Huỳnh Thị Oanh', '0910840481', '1992-07-10', 'Bình Định', 'Đang học', 'Nữ', 'huynh.thi.oanh@example.com', '402813743790', 'avatar_12.jpg'),
(16, 5, 1, 'Dương Văn Tuấn', '0930965419', '2004-04-06', 'Bình Định', 'Đang học', 'Nam', 'duong.van.tuan@example.com', '903107784770', 'avatar_13.jpg'),
(17, 1, 1, 'Dương Thị Hồng', '0934606438', '1997-07-17', 'Nha Trang', 'Đang học', 'Nữ', 'duong.thi.hong@example.com', '299969123142', 'avatar_14.jpg'),
(18, 3, 1, 'Đặng Thị Thu', '0957256873', '1991-08-19', 'Đà Nẵng', 'Đang học', 'Nữ', 'dang.thi.thu@example.com', '744566000316', 'avatar_15.jpg'),
(19, 4, 1, 'Dương Thị Trang', '0993660968', '1992-02-18', 'Thái Nguyên', 'Đang học', 'Nữ', 'duong.thi.trang@example.com', '472281120301', 'avatar_16.jpg'),
(20, 3, 1, 'Nguyễn Văn Bảo', '0919105948', '1990-04-02', 'Hải Phòng', 'Đang học', 'Nam', 'nguyen.van.bao@example.com', '374338315193', 'avatar_17.jpg'),
(21, 7, 1, 'Lê Văn An', '0927249571', '1995-10-18', 'Quảng Ngãi', 'Đang học', 'Nam', 'le.van.an@example.com', '116716159078', 'avatar_18.jpg'),
(22, 1, 1, 'Lý Văn Huy', '0945362244', '1997-07-07', 'Hải Phòng', 'Đang học', 'Nam', 'ly.van.huy@example.com', '874003265039', 'avatar_19.jpg'),
(23, 5, 1, 'Vũ Văn Đức', '0986667155', '2005-10-14', 'Hồ Chí Minh', 'Đang học', 'Nam', 'vu.van.duc@example.com', '942076918765', 'avatar_20.jpg'),
(24, 5, 2, 'Võ Văn Huy', '0985130154', '1992-07-02', 'Quảng Ngãi', 'Đang học', 'Nam', 'vo.van.huy@example.com', '970471255055', 'avatar_21.jpg'),
(25, 9, 2, 'Nguyễn Văn Đạt', '0988165182', '2001-08-07', 'Hà Nội', 'Đang học', 'Nam', 'nguyen.van.dat@example.com', '111410384072', 'avatar_22.jpg'),
(26, 9, 2, 'Phan Thị Mai', '0953457661', '1998-12-01', 'Nha Trang', 'Đang học', 'Nữ', 'phan.thi.mai@example.com', '249652349826', 'avatar_23.jpg'),
(27, 9, 2, 'Đỗ Văn Anh', '0956526100', '2002-03-17', 'Hà Nội', 'Đang học', 'Nam', 'do.van.anh@example.com', '088755768124', 'avatar_24.jpg'),
(28, 8, 2, 'Lê Thị Oanh', '0913782886', '1993-11-22', 'Quảng Ninh', 'Đang học', 'Nữ', 'le.thi.oanh@example.com', '836428813620', 'avatar_25.jpg'),
(29, 2, 2, 'Đặng Văn Huy', '0944505045', '1990-11-18', 'Vũng Tàu', 'Đang học', 'Nam', 'dang.van.huy@example.com', '716100597038', 'avatar_26.jpg'),
(30, 6, 2, 'Đỗ Văn Anh', '0968988097', '1999-05-20', 'Quảng Ngãi', 'Đang học', 'Nam', 'do.van.anh@example.com', '637120833939', 'avatar_27.jpg'),
(31, 6, 2, 'Vũ Văn Huy', '0967609213', '1995-07-16', 'Nha Trang', 'Đang học', 'Nam', 'vu.van.huy@example.com', '887951281121', 'avatar_28.jpg'),
(32, 1, 2, 'Phan Thị Lan', '0956643439', '2004-05-11', 'Hà Nội', 'Đang học', 'Nữ', 'phan.thi.lan@example.com', '759148636710', 'avatar_29.jpg'),
(33, 8, 2, 'Trần Thị Cúc', '0949835050', '1998-09-15', 'Quảng Ninh', 'Đang học', 'Nữ', 'tran.thi.cuc@example.com', '799876843417', 'avatar_30.jpg'),
(34, 2, 2, 'Ngô Thị Oanh', '0947556300', '1992-06-17', 'Quảng Ninh', 'Đang học', 'Nữ', 'ngo.thi.oanh@example.com', '758103976220', 'avatar_31.jpg'),
(35, 7, 2, 'Đỗ Văn Huy', '0919963209', '2003-08-14', 'Hải Phòng', 'Đang học', 'Nam', 'do.van.huy@example.com', '567476583871', 'avatar_32.jpg'),
(36, 3, 2, 'Dương Thị Trang', '0990404981', '2005-04-17', 'Lạng Sơn', 'Đang học', 'Nữ', 'duong.thi.trang@example.com', '972253106210', 'avatar_33.jpg'),
(37, 2, 2, 'Vũ Văn Huy', '0936606950', '1991-09-24', 'Quảng Ninh', 'Đang học', 'Nam', 'vu.van.huy@example.com', '468161421941', 'avatar_34.jpg'),
(38, 8, 2, 'Hồ Thị Cúc', '0917968115', '2000-07-26', 'Hồ Chí Minh', 'Đang học', 'Nữ', 'ho.thi.cuc@example.com', '971052568026', 'avatar_35.jpg'),
(39, 10, 2, 'Huỳnh Văn Huy', '0977161562', '1992-08-20', 'Cần Thơ', 'Đang học', 'Nam', 'huynh.van.huy@example.com', '916355009153', 'avatar_36.jpg'),
(40, 8, 2, 'Lý Văn Bảo', '0941593382', '1997-03-05', 'Hải Phòng', 'Đang học', 'Nam', 'ly.van.bao@example.com', '388893186791', 'avatar_37.jpg'),
(41, 9, 2, 'Phạm Văn Minh', '0947797190', '1999-01-25', 'Hà Nội', 'Đang học', 'Nam', 'pham.van.minh@example.com', '670702682587', 'avatar_38.jpg'),
(42, 9, 2, 'Nguyễn Văn Bình', '0949116879', '1992-06-27', 'Quảng Ninh', 'Đang học', 'Nam', 'nguyen.van.binh@example.com', '732817687083', 'avatar_39.jpg'),
(43, 7, 2, 'Võ Thị Oanh', '0999118050', '1994-08-21', 'Hà Nội', 'Đang học', 'Nữ', 'vo.thi.oanh@example.com', '405800119293', 'avatar_40.jpg'),
(44, 3, 3, 'Huỳnh Thị Lan', '0952482668', '1991-01-16', 'Đà Nẵng', 'Đang học', 'Nữ', 'huynh.thi.lan@example.com', '866429306409', 'avatar_41.jpg'),
(45, 4, 3, 'Đặng Thị Trang', '0951300021', '2000-05-02', 'Huế', 'Đang học', 'Nữ', 'dang.thi.trang@example.com', '566603610522', 'avatar_42.jpg'),
(46, 6, 3, 'Hoàng Văn Anh', '0943943811', '1999-09-09', 'Phú Yên', 'Đang học', 'Nam', 'hoang.van.anh@example.com', '074468842971', 'avatar_43.jpg'),
(47, 2, 3, 'Hồ Thị Mai', '0988910984', '1996-02-09', 'Đà Nẵng', 'Đang học', 'Nữ', 'ho.thi.mai@example.com', '283183080740', 'avatar_44.jpg'),
(48, 5, 3, 'Huỳnh Thị Bình', '0989703958', '1995-04-12', 'Vũng Tàu', 'Đang học', 'Nữ', 'huynh.thi.binh@example.com', '882214154933', 'avatar_45.jpg'),
(49, 7, 3, 'Đặng Thị Nga', '0939986732', '1996-12-02', 'Quảng Ngãi', 'Đang học', 'Nữ', 'dang.thi.nga@example.com', '985659665331', 'avatar_46.jpg'),
(50, 5, 3, 'Hồ Văn Minh', '0974541384', '2000-07-24', 'Quảng Ngãi', 'Đang học', 'Nam', 'ho.van.minh@example.com', '870426297802', 'avatar_47.jpg'),
(51, 9, 3, 'Hoàng Thị Trang', '0939662210', '1991-11-18', 'Huế', 'Đang học', 'Nữ', 'hoang.thi.trang@example.com', '657127190993', 'avatar_48.jpg'),
(52, 9, 3, 'Phạm Thị Oanh', '0972362015', '1998-02-20', 'Lạng Sơn', 'Đang học', 'Nữ', 'pham.thi.oanh@example.com', '166426123202', 'avatar_49.jpg'),
(53, 1, 3, 'Vũ Thị Ngọc', '0977818390', '2000-08-14', 'Nha Trang', 'Đang học', 'Nữ', 'vu.thi.ngoc@example.com', '060864690981', 'avatar_50.jpg'),
(54, 9, 3, 'Ngô Văn An', '0993289786', '2001-05-16', 'Phú Yên', 'Đang học', 'Nam', 'ngo.van.an@example.com', '608367516943', 'avatar_51.jpg'),
(55, 7, 3, 'Lê Thị Mai', '0998110612', '1993-12-28', 'Hà Nội', 'Đang học', 'Nữ', 'le.thi.mai@example.com', '039539881185', 'avatar_52.jpg'),
(56, 4, 3, 'Nguyễn Văn Hùng', '0933622049', '2002-12-27', 'Bình Định', 'Đang học', 'Nam', 'nguyen.van.hung@example.com', '739299531143', 'avatar_53.jpg'),
(57, 6, 3, 'Bùi Văn Anh', '0948235132', '2002-02-09', 'Thái Nguyên', 'Đang học', 'Nam', 'bui.van.anh@example.com', '334554567782', 'avatar_54.jpg'),
(58, 8, 3, 'Đặng Văn Tuấn', '0979373867', '1994-05-03', 'Phú Yên', 'Đang học', 'Nam', 'dang.van.tuan@example.com', '295510107255', 'avatar_55.jpg'),
(59, 3, 3, 'Phạm Văn Huy', '0997997231', '1990-04-14', 'Phú Yên', 'Đang học', 'Nam', 'pham.van.huy@example.com', '894643342630', 'avatar_56.jpg'),
(60, 4, 3, 'Ngô Văn Anh', '0924517078', '1995-11-12', 'Lạng Sơn', 'Đang học', 'Nam', 'ngo.van.anh@example.com', '458996509385', 'avatar_57.jpg'),
(61, 4, 3, 'Lý Văn Đức', '0931860270', '2005-03-17', 'Cần Thơ', 'Đang học', 'Nam', 'ly.van.duc@example.com', '771314174192', 'avatar_58.jpg'),
(62, 9, 3, 'Lê Thị Bình', '0934974402', '1990-11-14', 'Quảng Ninh', 'Đang học', 'Nữ', 'le.thi.binh@example.com', '082493735583', 'avatar_59.jpg'),
(63, 3, 3, 'Lê Thị Oanh', '0934385625', '1996-02-24', 'Phú Yên', 'Đang học', 'Nữ', 'le.thi.oanh@example.com', '277314520877', 'avatar_60.jpg'),
(64, 6, 4, 'Bùi Văn Khang', '0940888729', '2002-10-16', 'Hà Nội', 'Đang học', 'Nam', 'bui.van.khang@example.com', '524755200562', 'avatar_61.jpg'),
(65, 9, 4, 'Lê Văn Giang', '0955208647', '1998-05-24', 'Đà Nẵng', 'Đang học', 'Nam', 'le.van.giang@example.com', '944557669590', 'avatar_62.jpg'),
(66, 1, 4, 'Huỳnh Văn Bình', '0961616561', '1995-02-01', 'Huế', 'Đang học', 'Nam', 'huynh.van.binh@example.com', '799570235139', 'avatar_63.jpg'),
(67, 2, 4, 'Ngô Văn Giang', '0994695374', '1999-06-24', 'Lạng Sơn', 'Đang học', 'Nam', 'ngo.van.giang@example.com', '696426549168', 'avatar_64.jpg'),
(68, 1, 4, 'Đặng Thị Cúc', '0925744583', '1994-10-22', 'Lạng Sơn', 'Đang học', 'Nữ', 'dang.thi.cuc@example.com', '074688902565', 'avatar_65.jpg'),
(69, 1, 4, 'Ngô Thị Lan', '0992555405', '1991-03-15', 'Huế', 'Đang học', 'Nữ', 'ngo.thi.lan@example.com', '720559664188', 'avatar_66.jpg'),
(70, 10, 4, 'Hoàng Văn Anh', '0943706812', '1994-02-22', 'Quảng Ngãi', 'Đang học', 'Nam', 'hoang.van.anh@example.com', '319549557863', 'avatar_67.jpg'),
(71, 4, 4, 'Hồ Văn Minh', '0953744269', '2003-08-09', 'Đà Nẵng', 'Đang học', 'Nam', 'ho.van.minh@example.com', '148679494738', 'avatar_68.jpg'),
(72, 7, 4, 'Hồ Văn Đức', '0938058555', '1990-01-16', 'Hải Phòng', 'Đang học', 'Nam', 'ho.van.duc@example.com', '895088328175', 'avatar_69.jpg'),
(73, 5, 4, 'Hồ Thị Mai', '0929531543', '1995-04-10', 'Bắc Ninh', 'Đang học', 'Nữ', 'ho.thi.mai@example.com', '513870546906', 'avatar_70.jpg'),
(74, 2, 4, 'Nguyễn Văn Cường', '0976547685', '2001-06-15', 'Bắc Ninh', 'Đang học', 'Nam', 'nguyen.van.cuong@example.com', '053170961662', 'avatar_71.jpg'),
(75, 8, 4, 'Hoàng Văn Cường', '0999611726', '1997-04-24', 'Hồ Chí Minh', 'Đang học', 'Nam', 'hoang.van.cuong@example.com', '003559424992', 'avatar_72.jpg'),
(76, 4, 4, 'Huỳnh Văn Bình', '0967573183', '1998-04-11', 'Đà Nẵng', 'Đang học', 'Nam', 'huynh.van.binh@example.com', '479932826903', 'avatar_73.jpg'),
(77, 1, 4, 'Huỳnh Thị Hoa', '0961158550', '1999-06-23', 'Bình Định', 'Đang học', 'Nữ', 'huynh.thi.hoa@example.com', '837219077636', 'avatar_74.jpg'),
(78, 5, 4, 'Ngô Thị Mỹ', '0936521110', '1995-07-08', 'Cần Thơ', 'Đang học', 'Nữ', 'ngo.thi.my@example.com', '374116089691', 'avatar_75.jpg'),
(79, 8, 4, 'Hồ Thị Thu', '0926189119', '1993-03-04', 'Đà Nẵng', 'Đang học', 'Nữ', 'ho.thi.thu@example.com', '961452555996', 'avatar_76.jpg'),
(80, 8, 4, 'Đặng Văn Hải', '0941564589', '1990-10-19', 'Đà Nẵng', 'Đang học', 'Nam', 'dang.van.hai@example.com', '154576379819', 'avatar_77.jpg'),
(81, 2, 4, 'Lý Văn Bình', '0914568334', '1993-05-10', 'Bình Định', 'Đang học', 'Nam', 'ly.van.binh@example.com', '163670131517', 'avatar_78.jpg'),
(82, 5, 4, 'Hồ Văn Huy', '0953934716', '1995-09-06', 'Thái Nguyên', 'Đang học', 'Nam', 'ho.van.huy@example.com', '993768649677', 'avatar_79.jpg'),
(83, 6, 4, 'Trần Thị Cúc', '0920175687', '2002-06-13', 'Lạng Sơn', 'Đang học', 'Nữ', 'tran.thi.cuc@example.com', '018738005379', 'avatar_80.jpg'),
(84, 9, 5, 'Phạm Thị Bình', '0922552462', '2000-11-10', 'Quảng Ninh', 'Đang học', 'Nữ', 'pham.thi.binh@example.com', '881680957734', 'avatar_81.jpg'),
(85, 7, 5, 'Võ Thị Oanh', '0986276236', '1995-05-17', 'Nha Trang', 'Đang học', 'Nữ', 'vo.thi.oanh@example.com', '920535857280', 'avatar_82.jpg'),
(86, 5, 5, 'Dương Văn Cường', '0917555451', '1997-02-01', 'Quảng Ngãi', 'Đang học', 'Nam', 'duong.van.cuong@example.com', '651917994714', 'avatar_83.jpg'),
(87, 2, 5, 'Ngô Thị Oanh', '0947950911', '2001-08-01', 'Hà Nội', 'Đang học', 'Nữ', 'ngo.thi.oanh@example.com', '835561995116', 'avatar_84.jpg'),
(88, 2, 5, 'Phạm Thị Linh', '0977952037', '1991-11-16', 'Huế', 'Đang học', 'Nữ', 'pham.thi.linh@example.com', '891403165604', 'avatar_85.jpg'),
(89, 5, 5, 'Lý Thị Bình', '0955900775', '1990-09-07', 'Quảng Ngãi', 'Đang học', 'Nữ', 'ly.thi.binh@example.com', '978006898232', 'avatar_86.jpg'),
(90, 8, 5, 'Nguyễn Thị Thu', '0972938415', '2003-05-25', 'Quảng Ninh', 'Đang học', 'Nữ', 'nguyen.thi.thu@example.com', '420024687708', 'avatar_87.jpg'),
(91, 10, 5, 'Lý Thị Bình', '0981172125', '2003-01-17', 'Quảng Ninh', 'Đang học', 'Nữ', 'ly.thi.binh@example.com', '128252962386', 'avatar_88.jpg'),
(92, 4, 5, 'Hoàng Văn Anh', '0983156972', '1996-04-01', 'Quảng Ngãi', 'Đang học', 'Nam', 'hoang.van.anh@example.com', '172741896644', 'avatar_89.jpg'),
(93, 6, 5, 'Bùi Văn Đạt', '0983257300', '2002-11-17', 'Quảng Ngãi', 'Đang học', 'Nam', 'bui.van.dat@example.com', '113644139259', 'avatar_90.jpg'),
(94, 7, 5, 'Phạm Thị Hương', '0923239510', '1990-12-06', 'Hồ Chí Minh', 'Đang học', 'Nữ', 'pham.thi.huong@example.com', '731773552699', 'avatar_91.jpg'),
(95, 5, 5, 'Bùi Văn Khang', '0974709599', '2005-12-27', 'Phú Yên', 'Đang học', 'Nam', 'bui.van.khang@example.com', '583730575249', 'avatar_92.jpg'),
(96, 7, 5, 'Hồ Thị Linh', '0955422008', '2000-09-08', 'Bình Định', 'Đang học', 'Nữ', 'ho.thi.linh@example.com', '390012645510', 'avatar_93.jpg'),
(97, 8, 5, 'Trần Thị Trang', '0982540897', '1999-10-18', 'Bắc Ninh', 'Đang học', 'Nữ', 'tran.thi.trang@example.com', '475594390616', 'avatar_94.jpg'),
(98, 4, 5, 'Lý Văn Đạt', '0940924200', '1996-08-17', 'Hải Phòng', 'Đang học', 'Nam', 'ly.van.dat@example.com', '837736155058', 'avatar_95.jpg'),
(99, 7, 5, 'Hồ Thị Bình', '0941698936', '1993-09-25', 'Đà Nẵng', 'Đang học', 'Nữ', 'ho.thi.binh@example.com', '588672583627', 'avatar_96.jpg'),
(100, 5, 5, 'Lý Thị Lan', '0930481636', '2002-07-02', 'Vũng Tàu', 'Đang học', 'Nữ', 'ly.thi.lan@example.com', '072531292777', 'avatar_97.jpg'),
(101, 10, 5, 'Dương Thị Ngọc', '0970096576', '1999-06-17', 'Phú Yên', 'Đang học', 'Nữ', 'duong.thi.ngoc@example.com', '617807046359', 'avatar_98.jpg'),
(102, 6, 5, 'Vũ Thị Cúc', '0988369020', '1998-03-07', 'Phú Yên', 'Đang học', 'Nữ', 'vu.thi.cuc@example.com', '996749806559', 'avatar_99.jpg'),
(103, 8, 5, 'Lý Thị Lan', '0952271441', '1992-04-26', 'Hải Phòng', 'Đang học', 'Nữ', 'ly.thi.lan@example.com', '009106398963', 'avatar_100.jpg');

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
(MaTK, MaKhoa, TenGV, NgaySinhGV, GioiTinhGV, SoDienThoai, Email, TrangThai, AnhDaiDienGV) VALUES
                                                                                               (1, 1, 'Nguyễn Văn An', '1980-03-15', 'Nam', '0912345678', 'an.nguyen@univ.edu.vn', 'Đang công tác', 'an.jpg'),
                                                                                               (2, 1, 'Trần Thị Bình', '1982-07-22', 'Nữ', '0923456789', 'binh.tran@univ.edu.vn', 'Đang công tác', 'binh.jpg'),
                                                                                               (2, 1, 'Lê Quang Huy', '1985-05-10', 'Nam', '0934567890', 'huy.le@univ.edu.vn', 'Đang công tác', 'huy.jpg'),
                                                                                               (2, 1, 'Phạm Minh Châu', '1983-09-30', 'Nữ', '0945678901', 'chau.pham@univ.edu.vn', 'Đang nghỉ phép', 'chau.jpg'),
                                                                                               (2, 1, 'Đỗ Thị Thu Hà', '1987-01-25', 'Nữ', '0956789012', 'ha.do@univ.edu.vn', 'Đang công tác', 'ha.jpg'),
                                                                                               (2, 2, 'Ngô Văn Dũng', '1979-11-12', 'Nam', '0967890123', 'dung.ngo@univ.edu.vn', 'Đang công tác', 'dung.jpg'),
                                                                                               (2, 2, 'Vũ Thị Mai', '1986-04-18', 'Nữ', '0978901234', 'mai.vu@univ.edu.vn', 'Đang công tác', 'mai.jpg'),
                                                                                               (2, 2, 'Bùi Anh Tuấn', '1984-08-05', 'Nam', '0989012345', 'tuan.bui@univ.edu.vn', 'Đang nghỉ phép', 'tuan.jpg'),
                                                                                               (2, 2, 'Hoàng Lan Phương', '1988-02-14', 'Nữ', '0990123456', 'phuong.hoang@univ.edu.vn', 'Đang công tác', 'phuong.jpg'),
                                                                                               (2, 2, 'Phan Văn Khánh', '1978-12-20', 'Nam', '0901234567', 'khanh.phan@univ.edu.vn', 'Đang công tác', 'khanh.jpg');

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
INSERT INTO NhomHocPhan (MaGV, MaHP, MaLichDK, MaLop, HocKy, Nam, SiSo) VALUES
(1, 1, 2, 1, 2, 2024, 50),
(1, 2, 3, 2, 1, 2025, 45),
(2, 3, 2, 3, 2, 2024, 55),
(2, 4, 3, 4, 1, 2025, 60),
(3, 5, 2, 5, 2, 2024, 40),
(3, 6, 3, 6, 1, 2025, 50),
(4, 7, 2, 7, 2, 2024, 48),
(4, 8, 3, 8, 1, 2025, 52),
(5, 9, 2, 9, 2, 2024, 47),
(5, 10, 3, 10, 1, 2025, 53),
(6, 1, 2, 2, 2, 2024, 60),
(6, 2, 3, 3, 1, 2025, 58),
(7, 3, 2, 4, 2, 2024, 42),
(7, 4, 3, 5, 1, 2025, 50),
(8, 5, 2, 1, 2, 2024, 45),
(8, 6, 3, 6, 1, 2025, 45);



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
INSERT INTO `KetQua` (`MaHP`, `MaSV`, `MaDQT`, `DiemThi`, `DiemHe4`, `DiemHe10`, `HocKy`, `Nam`)
VALUES
    (1, 1, 1, 7.5, 3.0, 7.5, 1, '2025'),
    (2, 2, 2, 8.0, 3.5, 8.0, 1, '2025'),
    (3, 2, 2, 8.0, 3.5, 8.0, 2, '2025'),
    (3, 3, 3, 6.0, 2.0, 6.0, 2, '2025'),
    (4, 4, 4, 9.0, 4.0, 9.0, 2, '2025'),
    (5, 5, 5, 5.5, 1.5, 5.5, 1, '2025'),
    (6, 6, 6, 7.0, 2.8, 7.0, 2, '2025'),
    (7, 7, 7, 8.5, 3.6, 8.5, 1, '2025'),
    (8, 8, 8, 4.0, 1.0, 4.0, 2, '2025'),
    (9, 9, 9, 6.5, 2.5, 6.5, 1, '2025'),
    (10, 10, 10, 9.5, 4.0, 9.5, 2, '2025');



-- 19
INSERT INTO DiemQuaTrinh (DiemSo)
VALUES
    (8.5),
    (7.2),
    (9.0),
    (6.8),
    (5.5),
    (4.2),
    (9.3),
    (7.9),
    (6.0),
    (8.8);


-- 20
INSERT INTO CotDiem (MaDQT, TenCotDiem, DiemSo, HeSo)
VALUES
    (1, 'Cột điểm 1', 8.5, 0.5),
    (2, 'Cột điểm 2', 7.0, 0.5),
    (3, 'Cột điểm 1', 9.0, 0.5),
    (4, 'Cột điểm 2', 6.8, 0.5),
    (5, 'Cột điểm 1', 5.5, 0.5),
    (6, 'Cột điểm 2', 4.2, 0.5),
    (7, 'Cột điểm 1', 9.3, 0.5),
    (8, 'Cột điểm 2', 7.9, 0.5),
    (9, 'Cột điểm 1', 6.0, 0.5),
    (10, 'Cột điểm 2', 8.8, 0.5);



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


-- 27
INSERT INTO NhomQuyen (TenNhomQuyen) VALUES
					('SinhVien'),
                                         ('Admin'),
                                         ('AnQuyen');

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
                                                    (2, 3, 'Xem'), (2, 3, 'Them'), (2, 3, 'Sua'), (2, 3, 'Xoa');

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
(1, 'sv100', '123456', 'Sinh viên');



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




ALTER TABLE `KetQua`
    ADD CONSTRAINT `KetQua_DiemQuaTrinh` FOREIGN KEY (MaDQT) REFERENCES `DiemQuaTrinh`(MaDQT);

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























