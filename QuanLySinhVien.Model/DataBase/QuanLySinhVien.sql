DROP
DATABASE `QuanLySinhVien`;
CREATE
DATABASE `QuanLySinhVien`;
USE
`QuanLySinhVien`;

-- 1
CREATE TABLE `SinhVien`
(
    `MaSV`          INT AUTO_INCREMENT PRIMARY KEY,
    `MaTK`          INT,
    `MaLop`         INT,
    `MaKhoaHoc`     INT,
    `TenSV`         VARCHAR(255),
    `SoDienThoaiSV` VARCHAR(255),
    `NgaySinhSV`    VARCHAR(255),
    `QueQuanSV`     VARCHAR(255),
    `TrangThaiSV`   VARCHAR(255),
    `GioiTinhSV`    VARCHAR(255),
    `EmailSV`       VARCHAR(255),
    `CCCDSV`        VARCHAR(255),
    `AnhDaiDienSV`  VARCHAR(255),
    Status          TINYINT DEFAULT 1
);

-- 2
CREATE TABLE Lop
(
    MaLop     INT AUTO_INCREMENT PRIMARY KEY,
    MaGV      INT,
    MaNganh   INT,
    TenLop    VARCHAR(255),
    SoLuongSV INT,
    Status    TINYINT DEFAULT 1
);

-- 3
CREATE TABLE GiangVien
(
    MaGV         INT AUTO_INCREMENT PRIMARY KEY,
    MaTK         INT,
    MaKhoa       INT,
    TenGV        VARCHAR(255),
    NgaySinhGV   VARCHAR(255),
    GioiTinhGV   VARCHAR(255),
    SoDienThoai  VARCHAR(255),
    Email        VARCHAR(255),
    AnhDaiDienGV VARCHAR(255),
    Status       TINYINT DEFAULT 1
);

-- 4
CREATE TABLE Khoa
(
    MaKhoa  INT AUTO_INCREMENT PRIMARY KEY,
    TenKHoa VARCHAR(255),
    Email   VARCHAR(255),
    DiaChi  VARCHAR(255),
    Status  TINYINT DEFAULT 1
);

-- 5
CREATE TABLE Nganh
(
    MaNganh  INT AUTO_INCREMENT PRIMARY KEY,
    MaKhoa   INT,
    TenNganh VARCHAR(255),
    Status   TINYINT DEFAULT 1
);

-- 6
CREATE TABLE ChuyenNganh
(
    MaCN    INT AUTO_INCREMENT PRIMARY KEY,
    MaNganh INT,
    TenCN   VARCHAR(255),
    Status  TINYINT DEFAULT 1
);

-- 7
create table HocPhiTinChi
(
    MaHPTC         INT AUTO_INCREMENT PRIMARY KEY,
    MaNganh        INT,
    SoTienMotTinChi double,
    ThoiGianApDung VARCHAR(255),
    Status         TINYINT DEFAULT 1
);


-- 8
create table HocPhiSV
(
    MaHP      INT AUTO_INCREMENT PRIMARY KEY,
    MaSV      INT,
    HocKy     INT,
    Nam       VARCHAR(255),
    TongHocPhi double,
    DaThu double,
    TrangThai VARCHAR(255),
    Status    TINYINT DEFAULT 1
);

-- 9
create table KhoaHoc
(
    MaKhoaHoc   INT AUTO_INCREMENT PRIMARY KEY,
    MaCKDT      INT, -- FK

    TenKhoaHoc  VARCHAR(255),
    NienKhoaHoc VARCHAR(255),
    Status      TINYINT DEFAULT 1
);

-- 10
create table ChuKyDaoTao
(
    MaCKDT     INT AUTO_INCREMENT PRIMARY KEY,
    NamBatDau  VARCHAR(255),
    NamKetThuc VARCHAR(255),
    Status     TINYINT DEFAULT 1
);

-- 11
CREATE TABLE `DangKy`
(
    `MaNHP` INT,
    `MaSV`  INT,
    PRIMARY KEY (`MaNHP`, `MaSV`),
    Status  TINYINT DEFAULT 1
);

-- 12
CREATE TABLE `NhomHocPhan`
(
    `MaNHP`    INT AUTO_INCREMENT PRIMARY KEY,
    `MaGV`     INT,
    `MaHP`     INT,
    `MaLichDK` INT     DEFAULT 1,
    `MaLop`    INT,
    `HocKy`    INT,
    `Nam`      VARCHAR(255),
    `SiSo`     INT,
    Status     TINYINT DEFAULT 1
);

-- 13
CREATE TABLE `LichHoc`
(
    `MaLH`        INT AUTO_INCREMENT PRIMARY KEY,
    `MaPH`        INT,
    `MaNHP`       INT,
    `Thu`         VARCHAR(255),
    `TietBatDau`  INT,
    `TuNgay`      date,
    `DenNgay`     date,
    `TietKetThuc` INT,
    `SoTiet`      INT,
    `Type`        VARCHAR(255) DEFAULT 'Lý thuyết',
    Status        TINYINT      DEFAULT 1
);

-- 14
CREATE TABLE `PhongHoc`
(
    `MaPH`      INT AUTO_INCREMENT PRIMARY KEY,
    `TenPH`     VARCHAR(255),
    `LoaiPH`    VARCHAR(255),
    `CoSo`      VARCHAR(255),
    `SucChua`   INT,
    `TinhTrang` VARCHAR(255),
    Status      TINYINT DEFAULT 1
);

-- 15
CREATE TABLE `HocPhan`
(
    `MaHP`           INT AUTO_INCREMENT PRIMARY KEY,
    `MaHPTruoc`      INT,
    `TenHP`          VARCHAR(255),
    `SoTinChi`       INT,
    `HeSoHocPhan`    VARCHAR(255),
    `SoTietLyThuyet` INT,
    `SoTietThucHanh` INT,
    Status           TINYINT DEFAULT 1
);


-- 17
-- điểm thi chưa nhập
CREATE TABLE `KetQua`
(
    `MaKQ`     INT AUTO_INCREMENT PRIMARY KEY,
    `MaHP`     INT,
    `MaSV`     INT,
    `DiemThi`  float   DEFAULT 0,
    `DiemHe4`  float   DEFAULT 0,
    `DiemHe10` float   DEFAULT 0,
    `HocKy`    INT,
    `Nam`      VARCHAR(255),
    Status     TINYINT DEFAULT 1
);

-- 19
CREATE TABLE `DiemQuaTrinh`
(
    `MaDQT`  INT AUTO_INCREMENT PRIMARY KEY,
    `MaKQ`   INT,
    `DiemSo` float,
    Status   TINYINT DEFAULT 1
);

-- 20
CREATE TABLE `CotDiem`
(
    `MaCD`       INT AUTO_INCREMENT PRIMARY KEY,
    `MaDQT`      INT,
    `TenCotDiem` VARCHAR(255),
    `DiemSo`     float,
    `HeSo`       float,
    Status       TINYINT DEFAULT 1
);

-- 21
CREATE TABLE `HocPhiHocPhan`
(
    `MaHPHP` INT AUTO_INCREMENT PRIMARY KEY,
    `MaSV`   INT,
    `MaHP`   INT,
    `TongTien` double,
    `HocKy`  int,
    `Nam`    VARCHAR(255),
    Status   TINYINT DEFAULT 1
);

-- 22
CREATE TABLE `CaThi`
(
    `MaCT`           INT AUTO_INCREMENT PRIMARY KEY,
    `MaHP`           INT,
    `MaPH`           INT,
    `Thu`            VARCHAR(255),
    `ThoiGianBatDau` VARCHAR(255),
    `ThoiLuong`      VARCHAR(255),
    `HocKy`          INT,
    `Nam`            VARCHAR(255),
    Status           TINYINT DEFAULT 1
);

-- 23
CREATE TABLE `CaThi_SinhVien`
(
    `MaCT` INT,
    `MaSV` INT,
    PRIMARY KEY (`MaCT`, `MaSV`),
    Status TINYINT DEFAULT 1
);

-- 24
CREATE TABLE chuongtrinhdaotao_hocphan
(
    MaCTDT INT,
    MaHP   INT,
    PRIMARY KEY (MaCTDT, MaHP),
    Status TINYINT DEFAULT 1
);

-- 25
create table chuongtrinhdaotao
(
    MaCTDT     INT AUTO_INCREMENT PRIMARY KEY,
    MaCKDT     INT, -- FK
    MaNganh    INT, -- FK

    TenCTDT    VARCHAR(255),
    LoaiHinhDT VARCHAR(255),
    TrinhDo    VARCHAR(255),
    Status     TINYINT DEFAULT 1
);

-- 26
CREATE TABLE TaiKhoan
(
    MaTK        INT AUTO_INCREMENT PRIMARY KEY,
    MaNQ        INT,
    TenDangNhap VARCHAR(255),
    MatKhau     VARCHAR(255),
    Type        VARCHAR(255) DEFAULT 'Quản trị viên',
    Status      TINYINT      DEFAULT 1
);

-- 27
CREATE TABLE NhomQuyen
(
    MaNQ         INT AUTO_INCREMENT  PRIMARY KEY,
    TenNhomQuyen VARCHAR(255),
    Status       TINYINT DEFAULT 1
);

-- 28
CREATE TABLE ChiTietQuyen
(
    MaCN     INT,
    MaNQ     INT,
    HanhDong VARCHAR(255),
    PRIMARY KEY (MaCN, MaNQ, HanhDong),
    Status   TINYINT DEFAULT 1
);

-- 29
CREATE TABLE ChucNang
(
    MaCN        INT AUTO_INCREMENT PRIMARY KEY,
    TenChucNang VARCHAR(255),
    Status      TINYINT DEFAULT 1
);

-- 30
CREATE TABLE LichDangKy
(
    MaLichDK        INT AUTO_INCREMENT PRIMARY KEY,
    ThoiGianBatDau  TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    ThoiGianKetThuc TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    Status          TINYINT   DEFAULT 1
);


-- ------------------------INSERT-----------------------------------
-- 1
INSERT INTO SinhVien (MaTK, MaLop, MaKhoaHoc, TenSV, SoDienThoaiSV, NgaySinhSV, QueQuanSV, TrangThaiSV, GioiTinhSV,
                      EmailSV, CCCDSV, AnhDaiDienSV)
VALUES (3, 2, 1, 'Nguyễn Ân', '0968188598', '24/06/1995', 'Bình Định', 'Đang học', 'Nam', 'an.sv@gmail.com',
        '528139535131', 'Views/img/portrait/default.jpg'),
       (5, 8, 1, 'Ngô Thị Thu', '0976545290', '27/10/1997', 'Vũng Tàu', 'Đang học', 'Nữ', 'ngo.thi.thu@gmail.com',
        '249229315668', 'Views/img/portrait/default.jpg'),
       (6, 4, 1, 'Trần Văn Anh', '0960877017', '13/12/1998', 'Huế', 'Đang học', 'Nam', 'tran.van.anh@gmail.com',
        '753662145005', 'Views/img/portrait/default.jpg'),
       (7, 6, 1, 'Dương Thị Lan', '0975025648', '18/03/2000', 'Nha Trang', 'Đang học', 'Nữ', 'duong.thi.lan@gmail.com',
        '254131630817', 'Views/img/portrait/default.jpg'),
       (8, 9, 1, 'Lý Thị Cúc', '0942677057', '16/05/2000', 'Phú Yên', 'Đang học', 'Nữ', 'ly.thi.cuc@gmail.com',
        '288784918817', 'Views/img/portrait/default.jpg'),
       (9, 9, 1, 'Võ Thị Nga', '0963229308', '01/04/1992', 'Hà Nội', 'Đang học', 'Nữ', 'vo.thi.nga@gmail.com',
        '139788401816', 'Views/img/portrait/default.jpg'),
       (10, 7, 1, 'Bùi Thị Lan', '0985119093', '04/04/2004', 'Cần Thơ', 'Đang học', 'Nữ', 'bui.thi.lan@gmail.com',
        '346597264307', 'Views/img/portrait/default.jpg'),
       (11, 10, 1, 'Bùi Thị Trang', '0975984367', '07/07/2001', 'Nha Trang', 'Đang học', 'Nữ',
        'bui.thi.trang@gmail.com', '557048851125', 'Views/img/portrait/default.jpg'),
       (12, 2, 1, 'Bùi Văn Anh', '0938185337', '23/10/1990', 'Lạng Sơn', 'Đang học', 'Nam', 'bui.van.anh@gmail.com',
        '729358637684', 'Views/img/portrait/default.jpg'),
       (13, 5, 1, 'Dương Văn Bình', '0987631890', '20/07/2003', 'Phú Yên', 'Đang học', 'Nam',
        'duong.van.binh@gmail.com', '952070995341', 'Views/img/portrait/default.jpg'),
       (14, 7, 1, 'Hoàng Văn Tuấn', '0954639950', '02/09/2002', 'Hải Phòng', 'Đang học', 'Nam',
        'hoang.van.tuan@gmail.com', '728526660271', 'Views/img/portrait/default.jpg'),
       (15, 7, 1, 'Huỳnh Thị Oanh', '0910840481', '10/07/1992', 'Bình Định', 'Đang học', 'Nữ',
        'huynh.thi.oanh@gmail.com', '402813743790', 'Views/img/portrait/default.jpg'),
       (16, 5, 1, 'Dương Văn Tuấn', '0930965419', '06/04/2004', 'Bình Định', 'Đang học', 'Nam',
        'duong.van.tuan@gmail.com', '903107784770', 'Views/img/portrait/default.jpg'),
       (17, 1, 1, 'Dương Thị Hồng', '0934606438', '17/07/1997', 'Nha Trang', 'Đang học', 'Nữ',
        'duong.thi.hong@gmail.com', '299969123142', 'Views/img/portrait/default.jpg'),
       (18, 3, 1, 'Đặng Thị Thu', '0957256873', '19/08/1991', 'Đà Nẵng', 'Đang học', 'Nữ', 'dang.thi.thu@gmail.com',
        '744566000316', 'Views/img/portrait/default.jpg'),
       (19, 4, 1, 'Dương Thị Trang', '0993660968', '18/02/1992', 'Thái Nguyên', 'Đang học', 'Nữ',
        'duong.thi.trang@gmail.com', '472281120301', 'Views/img/portrait/default.jpg'),
       (20, 3, 1, 'Nguyễn Văn Bảo', '0919105948', '02/04/1990', 'Hải Phòng', 'Đang học', 'Nam',
        'nguyen.van.bao@gmail.com', '374338315193', 'Views/img/portrait/default.jpg'),
       (21, 7, 1, 'Lê Văn An', '0927249571', '18/10/1995', 'Quảng Ngãi', 'Đang học', 'Nam', 'le.van.an@gmail.com',
        '116716159078', 'Views/img/portrait/default.jpg'),
       (22, 1, 1, 'Lý Văn Huy', '0945362244', '07/07/1997', 'Hải Phòng', 'Đang học', 'Nam', 'ly.van.huy@gmail.com',
        '874003265039', 'Views/img/portrait/default.jpg'),
       (23, 5, 1, 'Vũ Văn Đức', '0986667155', '14/10/2005', 'Hồ Chí Minh', 'Đang học', 'Nam', 'vu.van.duc@gmail.com',
        '942076918765', 'Views/img/portrait/default.jpg'),
       (24, 5, 2, 'Võ Văn Huy', '0985130154', '02/07/1992', 'Quảng Ngãi', 'Đang học', 'Nam', 'vo.van.huy@gmail.com',
        '970471255055', 'Views/img/portrait/default.jpg'),
       (25, 9, 2, 'Nguyễn Văn Đạt', '0988165182', '07/08/2001', 'Hà Nội', 'Đang học', 'Nam', 'nguyen.van.dat@gmail.com',
        '111410384072', 'Views/img/portrait/default.jpg'),
       (26, 9, 2, 'Phan Thị Mai', '0953457661', '01/12/1998', 'Nha Trang', 'Đang học', 'Nữ', 'phan.thi.mai@gmail.com',
        '249652349826', 'Views/img/portrait/default.jpg'),
       (27, 9, 2, 'Đỗ Văn Anh', '0956526100', '17/03/2002', 'Hà Nội', 'Đang học', 'Nam', 'do.van.anh@gmail.com',
        '088755768124', 'Views/img/portrait/default.jpg'),
       (28, 8, 2, 'Lê Thị Oanh', '0913782886', '22/11/1993', 'Quảng Ninh', 'Đang học', 'Nữ', 'le.thi.oanh@gmail.com',
        '836428813620', 'Views/img/portrait/default.jpg'),
       (29, 2, 2, 'Đặng Văn Huy', '0944505045', '18/11/1990', 'Vũng Tàu', 'Đang học', 'Nam', 'dang.van.huy@gmail.com',
        '716100597038', 'Views/img/portrait/default.jpg'),
       (30, 6, 2, 'Đỗ Văn Anh', '0968988097', '20/05/1999', 'Quảng Ngãi', 'Đang học', 'Nam', 'do.van.anh@gmail.com',
        '637120833939', 'Views/img/portrait/default.jpg'),
       (31, 6, 2, 'Vũ Văn Huy', '0967609213', '16/07/1995', 'Nha Trang', 'Đang học', 'Nam', 'vu.van.huy@gmail.com',
        '887951281121', 'Views/img/portrait/default.jpg'),
       (32, 1, 2, 'Phan Thị Lan', '0956643439', '11/05/2004', 'Hà Nội', 'Đang học', 'Nữ', 'phan.thi.lan@gmail.com',
        '759148636710', 'Views/img/portrait/default.jpg'),
       (33, 8, 2, 'Trần Thị Cúc', '0949835050', '15/09/1998', 'Quảng Ninh', 'Đang học', 'Nữ', 'tran.thi.cuc@gmail.com',
        '799876843417', 'Views/img/portrait/default.jpg'),
       (34, 2, 2, 'Ngô Thị Oanh', '0947556300', '17/06/1992', 'Quảng Ninh', 'Đang học', 'Nữ', 'ngo.thi.oanh@gmail.com',
        '758103976220', 'Views/img/portrait/default.jpg'),
       (35, 7, 2, 'Đỗ Văn Huy', '0919963209', '14/08/2003', 'Hải Phòng', 'Đang học', 'Nam', 'do.van.huy@gmail.com',
        '567476583871', 'Views/img/portrait/default.jpg'),
       (36, 3, 2, 'Dương Thị Trang', '0990404981', '17/04/2005', 'Lạng Sơn', 'Đang học', 'Nữ',
        'duong.thi.trang@gmail.com', '972253106210', 'Views/img/portrait/default.jpg'),
       (37, 2, 2, 'Vũ Văn Huy', '0936606950', '24/09/1991', 'Quảng Ninh', 'Đang học', 'Nam', 'vu.van.huy@gmail.com',
        '468161421941', 'Views/img/portrait/default.jpg'),
       (38, 8, 2, 'Hồ Thị Cúc', '0917968115', '26/07/2000', 'Hồ Chí Minh', 'Đang học', 'Nữ', 'ho.thi.cuc@gmail.com',
        '971052568026', 'Views/img/portrait/default.jpg'),
       (39, 10, 2, 'Huỳnh Văn Huy', '0977161562', '20/08/1992', 'Cần Thơ', 'Đang học', 'Nam', 'huynh.van.huy@gmail.com',
        '916355009153', 'Views/img/portrait/default.jpg'),
       (40, 8, 2, 'Lý Văn Bảo', '0941593382', '05/03/1997', 'Hải Phòng', 'Đang học', 'Nam', 'ly.van.bao@gmail.com',
        '388893186791', 'Views/img/portrait/default.jpg'),
       (41, 9, 2, 'Phạm Văn Minh', '0947797190', '25/01/1999', 'Hà Nội', 'Đang học', 'Nam', 'pham.van.minh@gmail.com',
        '670702682587', 'Views/img/portrait/default.jpg'),
       (42, 9, 2, 'Nguyễn Văn Bình', '0949116879', '27/06/1992', 'Quảng Ninh', 'Đang học', 'Nam',
        'nguyen.van.binh@gmail.com', '732817687083', 'Views/img/portrait/default.jpg'),
       (43, 7, 2, 'Võ Thị Oanh', '0999118050', '21/08/1994', 'Hà Nội', 'Đang học', 'Nữ', 'vo.thi.oanh@gmail.com',
        '405800119293', 'Views/img/portrait/default.jpg'),
       (44, 3, 3, 'Huỳnh Thị Lan', '0952482668', '16/01/1991', 'Đà Nẵng', 'Đang học', 'Nữ', 'huynh.thi.lan@gmail.com',
        '866429306409', 'Views/img/portrait/default.jpg'),
       (45, 4, 3, 'Đặng Thị Trang', '0951300021', '02/05/2000', 'Huế', 'Đang học', 'Nữ', 'dang.thi.trang@gmail.com',
        '566603610522', 'Views/img/portrait/default.jpg'),
       (46, 6, 3, 'Hoàng Văn Anh', '0943943811', '09/09/1999', 'Phú Yên', 'Đang học', 'Nam', 'hoang.van.anh@gmail.com',
        '074468842971', 'Views/img/portrait/default.jpg'),
       (47, 2, 3, 'Hồ Thị Mai', '0988910984', '09/02/1996', 'Đà Nẵng', 'Đang học', 'Nữ', 'ho.thi.mai@gmail.com',
        '283183080740', 'Views/img/portrait/default.jpg'),
       (48, 5, 3, 'Huỳnh Thị Bình', '0989703958', '12/04/1995', 'Vũng Tàu', 'Đang học', 'Nữ',
        'huynh.thi.binh@gmail.com', '882214154933', 'Views/img/portrait/default.jpg'),
       (49, 7, 3, 'Đặng Thị Nga', '0939986732', '02/12/1996', 'Quảng Ngãi', 'Đang học', 'Nữ', 'dang.thi.nga@gmail.com',
        '985659665331', 'Views/img/portrait/default.jpg'),
       (50, 5, 3, 'Hồ Văn Minh', '0974541384', '24/07/2000', 'Quảng Ngãi', 'Đang học', 'Nam', 'ho.van.minh@gmail.com',
        '870426297802', 'Views/img/portrait/default.jpg'),
       (51, 9, 3, 'Hoàng Thị Trang', '0939662210', '18/11/1991', 'Huế', 'Đang học', 'Nữ', 'hoang.thi.trang@gmail.com',
        '657127190993', 'Views/img/portrait/default.jpg'),
       (52, 9, 3, 'Phạm Thị Oanh', '0972362015', '20/02/1998', 'Lạng Sơn', 'Đang học', 'Nữ', 'pham.thi.oanh@gmail.com',
        '166426123202', 'Views/img/portrait/default.jpg'),
       (53, 1, 3, 'Vũ Thị Ngọc', '0977818390', '14/08/2000', 'Nha Trang', 'Đang học', 'Nữ', 'vu.thi.ngoc@gmail.com',
        '060864690981', 'Views/img/portrait/default.jpg'),
       (54, 9, 3, 'Ngô Văn An', '0993289786', '16/05/2001', 'Phú Yên', 'Đang học', 'Nam', 'ngo.van.an@gmail.com',
        '608367516943', 'Views/img/portrait/default.jpg'),
       (55, 7, 3, 'Lê Thị Mai', '0998110612', '28/12/1993', 'Hà Nội', 'Đang học', 'Nữ', 'le.thi.mai@gmail.com',
        '039539881185', 'Views/img/portrait/default.jpg'),
       (56, 4, 3, 'Nguyễn Văn Hùng', '0933622049', '27/12/2002', 'Bình Định', 'Đang học', 'Nam',
        'nguyen.van.hung@gmail.com', '739299531143', 'Views/img/portrait/default.jpg'),
       (57, 6, 3, 'Bùi Văn Anh', '0948235132', '09/02/2002', 'Thái Nguyên', 'Đang học', 'Nam', 'bui.van.anh@gmail.com',
        '334554567782', 'Views/img/portrait/default.jpg'),
       (58, 8, 3, 'Đặng Văn Tuấn', '0979373867', '03/05/1994', 'Phú Yên', 'Đang học', 'Nam', 'dang.van.tuan@gmail.com',
        '295510107255', 'Views/img/portrait/default.jpg'),
       (59, 3, 3, 'Phạm Văn Huy', '0997997231', '14/04/1990', 'Phú Yên', 'Đang học', 'Nam', 'pham.van.huy@gmail.com',
        '894643342630', 'Views/img/portrait/default.jpg'),
       (60, 4, 3, 'Ngô Văn Anh', '0924517078', '12/11/1995', 'Lạng Sơn', 'Đang học', 'Nam', 'ngo.van.anh@gmail.com',
        '458996509385', 'Views/img/portrait/default.jpg'),
       (61, 4, 3, 'Lý Văn Đức', '0931860270', '17/03/2005', 'Cần Thơ', 'Đang học', 'Nam', 'ly.van.duc@gmail.com',
        '771314174192', 'Views/img/portrait/default.jpg'),
       (62, 9, 3, 'Lê Thị Bình', '0934974402', '14/11/1990', 'Quảng Ninh', 'Đang học', 'Nữ', 'le.thi.binh@gmail.com',
        '082493735583', 'Views/img/portrait/default.jpg'),
       (63, 3, 3, 'Lê Thị Oanh', '0934385625', '24/02/1996', 'Phú Yên', 'Đang học', 'Nữ', 'le.thi.oanh@gmail.com',
        '277314520877', 'Views/img/portrait/default.jpg'),
       (64, 6, 4, 'Bùi Văn Khang', '0940888729', '16/10/2002', 'Hà Nội', 'Đang học', 'Nam', 'bui.van.khang@gmail.com',
        '524755200562', 'Views/img/portrait/default.jpg'),
       (65, 9, 4, 'Lê Văn Giang', '0955208647', '24/05/1998', 'Đà Nẵng', 'Đang học', 'Nam', 'le.van.giang@gmail.com',
        '944557669590', 'Views/img/portrait/default.jpg'),
       (66, 1, 4, 'Huỳnh Văn Bình', '0961616561', '01/02/1995', 'Huế', 'Đang học', 'Nam', 'huynh.van.binh@gmail.com',
        '799570235139', 'Views/img/portrait/default.jpg'),
       (67, 2, 4, 'Ngô Văn Giang', '0994695374', '24/06/1999', 'Lạng Sơn', 'Đang học', 'Nam', 'ngo.van.giang@gmail.com',
        '696426549168', 'Views/img/portrait/default.jpg'),
       (68, 1, 4, 'Đặng Thị Cúc', '0925744583', '22/10/1994', 'Lạng Sơn', 'Đang học', 'Nữ', 'dang.thi.cuc@gmail.com',
        '074688902565', 'Views/img/portrait/default.jpg'),
       (69, 1, 4, 'Ngô Thị Lan', '0992555405', '15/03/1991', 'Huế', 'Đang học', 'Nữ', 'ngo.thi.lan@gmail.com',
        '720559664188', 'Views/img/portrait/default.jpg'),
       (70, 10, 4, 'Hoàng Văn Anh', '0943706812', '22/02/1994', 'Quảng Ngãi', 'Đang học', 'Nam',
        'hoang.van.anh@gmail.com', '319549557863', 'Views/img/portrait/default.jpg'),
       (71, 4, 4, 'Hồ Văn Minh', '0953744269', '09/08/2003', 'Đà Nẵng', 'Đang học', 'Nam', 'ho.van.minh@gmail.com',
        '148679494738', 'Views/img/portrait/default.jpg'),
       (72, 7, 4, 'Hồ Văn Đức', '0938058555', '16/01/1990', 'Hải Phòng', 'Đang học', 'Nam', 'ho.van.duc@gmail.com',
        '895088328175', 'Views/img/portrait/default.jpg'),
       (73, 5, 4, 'Hồ Thị Mai', '0929531543', '10/04/1995', 'Bắc Ninh', 'Đang học', 'Nữ', 'ho.thi.mai@gmail.com',
        '513870546906', 'Views/img/portrait/default.jpg'),
       (74, 2, 4, 'Nguyễn Văn Cường', '0976547685', '15/06/2001', 'Bắc Ninh', 'Đang học', 'Nam',
        'nguyen.van.cuong@gmail.com', '053170961662', 'Views/img/portrait/default.jpg'),
       (75, 8, 4, 'Hoàng Văn Cường', '0999611726', '24/04/1997', 'Hồ Chí Minh', 'Đang học', 'Nam',
        'hoang.van.cuong@gmail.com', '003559424992', 'Views/img/portrait/default.jpg'),
       (76, 4, 4, 'Huỳnh Văn Bình', '0967573183', '11/04/1998', 'Đà Nẵng', 'Đang học', 'Nam',
        'huynh.van.binh@gmail.com', '479932826903', 'Views/img/portrait/default.jpg'),
       (77, 1, 4, 'Huỳnh Thị Hoa', '0961158550', '23/06/1999', 'Bình Định', 'Đang học', 'Nữ', 'huynh.thi.hoa@gmail.com',
        '837219077636', 'Views/img/portrait/default.jpg'),
       (78, 5, 4, 'Ngô Thị Mỹ', '0936521110', '08/07/1995', 'Cần Thơ', 'Đang học', 'Nữ', 'ngo.thi.my@gmail.com',
        '374116089691', 'Views/img/portrait/default.jpg'),
       (79, 8, 4, 'Hồ Thị Thu', '0926189119', '04/03/1993', 'Đà Nẵng', 'Đang học', 'Nữ', 'ho.thi.thu@gmail.com',
        '961452555996', 'Views/img/portrait/default.jpg'),
       (80, 8, 4, 'Đặng Văn Hải', '0941564589', '19/10/1990', 'Đà Nẵng', 'Đang học', 'Nam', 'dang.van.hai@gmail.com',
        '154576379819', 'Views/img/portrait/default.jpg'),
       (81, 2, 4, 'Lý Văn Bình', '0914568334', '10/05/1993', 'Bình Định', 'Đang học', 'Nam', 'ly.van.binh@gmail.com',
        '163670131517', 'Views/img/portrait/default.jpg'),
       (82, 5, 4, 'Hồ Văn Huy', '0953934716', '06/09/1995', 'Thái Nguyên', 'Đang học', 'Nam', 'ho.van.huy@gmail.com',
        '993768649677', 'Views/img/portrait/default.jpg'),
       (83, 6, 4, 'Trần Thị Cúc', '0920175687', '13/06/2002', 'Lạng Sơn', 'Đang học', 'Nữ', 'tran.thi.cuc@gmail.com',
        '018738005379', 'Views/img/portrait/default.jpg'),
       (84, 9, 5, 'Phạm Thị Bình', '0922552462', '10/11/2000', 'Quảng Ninh', 'Đang học', 'Nữ',
        'pham.thi.binh@gmail.com', '881680957734', 'Views/img/portrait/default.jpg'),
       (85, 7, 5, 'Võ Thị Oanh', '0986276236', '17/05/1995', 'Nha Trang', 'Đang học', 'Nữ', 'vo.thi.oanh@gmail.com',
        '920535857280', 'Views/img/portrait/default.jpg'),
       (86, 5, 5, 'Dương Văn Cường', '0917555451', '01/02/1997', 'Quảng Ngãi', 'Đang học', 'Nam',
        'duong.van.cuong@gmail.com', '651917994714', 'Views/img/portrait/default.jpg'),
       (87, 2, 5, 'Ngô Thị Oanh', '0947950911', '01/08/2001', 'Hà Nội', 'Đang học', 'Nữ', 'ngo.thi.oanh@gmail.com',
        '835561995116', 'Views/img/portrait/default.jpg'),
       (88, 2, 5, 'Phạm Thị Linh', '0977952037', '16/11/1991', 'Huế', 'Đang học', 'Nữ', 'pham.thi.linh@gmail.com',
        '891403165604', 'Views/img/portrait/default.jpg'),
       (89, 5, 5, 'Lý Thị Bình', '0955900775', '07/09/1990', 'Quảng Ngãi', 'Đang học', 'Nữ', 'ly.thi.binh@gmail.com',
        '978006898232', 'Views/img/portrait/default.jpg'),
       (90, 8, 5, 'Nguyễn Thị Thu', '0972938415', '25/05/2003', 'Quảng Ninh', 'Đang học', 'Nữ',
        'nguyen.thi.thu@gmail.com', '420024687708', 'Views/img/portrait/default.jpg'),
       (91, 10, 5, 'Lý Thị Bình', '0981172125', '17/01/2003', 'Quảng Ninh', 'Đang học', 'Nữ', 'ly.thi.binh@gmail.com',
        '128252962386', 'Views/img/portrait/default.jpg'),
       (92, 4, 5, 'Hoàng Văn Anh', '0983156972', '01/04/1996', 'Quảng Ngãi', 'Đang học', 'Nam',
        'hoang.van.anh@gmail.com', '172741896644', 'Views/img/portrait/default.jpg'),
       (93, 6, 5, 'Bùi Văn Đạt', '0983257300', '17/11/2002', 'Quảng Ngãi', 'Đang học', 'Nam', 'bui.van.dat@gmail.com',
        '113644139259', 'Views/img/portrait/default.jpg'),
       (94, 7, 5, 'Phạm Thị Hương', '0923239510', '06/12/1990', 'Hồ Chí Minh', 'Đang học', 'Nữ',
        'pham.thi.huong@gmail.com', '731773552699', 'Views/img/portrait/default.jpg'),
       (95, 5, 5, 'Bùi Văn Khang', '0974709599', '27/12/2005', 'Phú Yên', 'Đang học', 'Nam', 'bui.van.khang@gmail.com',
        '583730575249', 'Views/img/portrait/default.jpg'),
       (96, 7, 5, 'Hồ Thị Linh', '0955422008', '08/09/2000', 'Bình Định', 'Đang học', 'Nữ', 'ho.thi.linh@gmail.com',
        '390012645510', 'Views/img/portrait/default.jpg'),
       (97, 8, 5, 'Trần Thị Trang', '0982540897', '18/10/1999', 'Bắc Ninh', 'Đang học', 'Nữ',
        'tran.thi.trang@gmail.com', '475594390616', 'Views/img/portrait/default.jpg'),
       (98, 4, 5, 'Lý Văn Đạt', '0940924200', '17/08/1996', 'Hải Phòng', 'Đang học', 'Nam', 'ly.van.dat@gmail.com',
        '837736155058', 'Views/img/portrait/default.jpg'),
       (99, 7, 5, 'Hồ Thị Bình', '0941698936', '25/09/1993', 'Đà Nẵng', 'Đang học', 'Nữ', 'ho.thi.binh@gmail.com',
        '588672583627', 'Views/img/portrait/default.jpg'),
       (100, 5, 5, 'Lý Thị Lan', '0930481636', '02/07/2002', 'Vũng Tàu', 'Đang học', 'Nữ', 'ly.thi.lan@gmail.com',
        '072531292777', 'Views/img/portrait/default.jpg'),
       (101, 10, 5, 'Dương Thị Ngọc', '0970096576', '17/06/1999', 'Phú Yên', 'Đang học', 'Nữ',
        'duong.thi.ngoc@gmail.com', '617807046359', 'Views/img/portrait/default.jpg'),
       (102, 6, 5, 'Vũ Thị Cúc', '0988369020', '07/03/1998', 'Phú Yên', 'Đang học', 'Nữ', 'vu.thi.cuc@gmail.com',
        '996749806559', 'Views/img/portrait/default.jpg'),
       (103, 8, 5, 'Lý Thị Lan', '0952271441', '26/04/1992', 'Hải Phòng', 'Đang học', 'Nữ', 'ly.thi.lan@gmail.com',
        '009106398963', 'Views/img/portrait/default.jpg');

-- 2
INSERT INTO Lop (MaGV, MaNganh, TenLop, SoLuongSV)
VALUES (1, 1, 'CNTT01', 50),
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
(MaTK, MaKhoa, TenGV, NgaySinhGV, GioiTinhGV, SoDienThoai, Email, AnhDaiDienGV)
VALUES (1, 1, 'Nguyễn Văn An', '15/03/1980', 'Nam', '0912345678', 'an.nguyen@univ.edu.vn', 'img/portrait/default.jpg'),
       (4, 1, 'Nguyễn Ân GV', '22/07/1982', 'Nữ', '0923456789', 'nguyen.an.gv@univ.edu.vn', 'img/portrait/default.jpg'),
       (2, 1, 'Lê Quang Huy', '10/05/1985', 'Nam', '0934567890', 'huy.le@univ.edu.vn', 'img/portrait/default.jpg'),
       (2, 1, 'Phạm Minh Châu', '30/09/1983', 'Nữ', '0945678901', 'chau.pham@univ.edu.vn', 'img/portrait/default.jpg'),
       (2, 1, 'Đỗ Thị Thu Hà', '25/01/1987', 'Nữ', '0956789012', 'ha.do@univ.edu.vn', 'img/portrait/default.jpg'),
       (2, 2, 'Ngô Văn Dũng', '12/11/1979', 'Nam', '0967890123', 'dung.ngo@univ.edu.vn', 'img/portrait/default.jpg'),
       (2, 2, 'Vũ Thị Mai', '18/04/1986', 'Nữ', '0978901234', 'mai.vu@univ.edu.vn', 'img/portrait/default.jpg'),
       (2, 2, 'Bùi Anh Tuấn', '05/08/1984', 'Nam', '0989012345', 'tuan.bui@univ.edu.vn', 'img/portrait/default.jpg'),
       (2, 2, 'Hoàng Lan Phương', '14/02/1988', 'Nữ', '0990123456', 'phuong.hoang@univ.edu.vn',
        'img/portrait/default.jpg'),
       (2, 2, 'Phan Văn Khánh', '20/12/1978', 'Nam', '0901234567', 'khanh.phan@univ.edu.vn',
        'img/portrait/default.jpg');


-- 4
INSERT INTO Khoa (TenKHoa, Email, DiaChi)
VALUES ('Công nghệ thông tin', 'cntt@khoa.edu', '123 Duong 1'),
       ('Kinh tế', 'kinhte@khoa.edu', '456 Duong 2');
-- 5
INSERT INTO Nganh (MaKhoa, TenNganh)
VALUES (1, 'Hệ thống thông tin'),
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
INSERT INTO ChuyenNganh (MaNganh, TenCN)
VALUES (1, 'Công nghệ phần mềm'),            -- Hệ thống thông tin
       (2, 'Tài chính công'),                -- Tài chính
       (3, 'Kiến trúc công trình dân dụng'), -- Xây dựng dân dụng (nếu em muốn bỏ, có thể giữ ngành khác)
       (4, 'Hệ thống nhúng'),                -- Mạng máy tính
       (5, 'Đầu tư quốc tế'),                -- Kinh tế quốc tế
       (6, 'Xây dựng hạ tầng kỹ thuật'),     -- Xây dựng cầu đường (nếu vẫn giữ ngành xây dựng)
       (7, 'Khoa học dữ liệu nâng cao'),     -- Trí tuệ nhân tạo
       (8, 'Bảo hiểm và quản lý rủi ro'),    -- Quản lý kinh tế
       (9, 'Quản lý dự án'),                 -- Kế toán
       (10, 'Bảo mật mạng');
-- Ngân hàng


-- 7
INSERT INTO HocPhiTinChi (MaNganh, SoTienMotTinChi, ThoiGianApDung)
VALUES (1, 480000, '15/05/2025 07:30:00'), -- Hệ thống thông tin
       (2, 420000, '15/05/2025 07:30:00'), -- Tài chính
       (3, 470000, '15/05/2025 07:30:00'), -- Xây dựng dân dụng
       (4, 500000, '15/05/2025 07:30:00'), -- Mạng máy tính
       (5, 430000, '15/05/2025 07:30:00'), -- Kinh tế quốc tế
       (6, 510000, '15/05/2025 07:30:00'), -- Trí tuệ nhân tạo
       (7, 410000, '15/05/2025 07:30:00'), -- Quản lý kinh tế
       (8, 520000, '15/05/2025 07:30:00'), -- Khoa học dữ liệu
       (9, 400000, '15/05/2025 07:30:00'), -- Kế toán
       (10, 415000, '15/05/2025 07:30:00');
-- Ngân hàng

-- 8
INSERT INTO HocPhiSV (MaSV, HocKy, Nam, TongHocPhi, DaThu, TrangThai)
VALUES (1, 1, '2025', 871000, 871000, 'Đã đóng'),
       (2, 1, '2025', 892000, 892000, 'Đã đóng'),
       (3, 1, '2025', 896000, 896000, 'Đã đóng'),
       (4, 1, '2025', 869000, 869000, 'Đã đóng'),
       (5, 1, '2025', 869000, 869000, 'Đã đóng'),
       (6, 1, '2025', 879000, 879000, 'Đã đóng'),
       (7, 1, '2025', 933000, 933000, 'Đã đóng'),
       (8, 1, '2025', 874000, 874000, 'Đã đóng'),
       (9, 1, '2025', 859000, 859000, 'Đã đóng'),
       (10, 1, '2025', 854000, 854000, 'Đã đóng'),
       (11, 1, '2025', 900000, 900000, 'Đã đóng'),
       (12, 1, '2025', 941000, 941000, 'Đã đóng'),
       (13, 1, '2025', 909000, 909000, 'Đã đóng'),
       (14, 1, '2025', 855000, 855000, 'Đã đóng'),
       (15, 1, '2025', 918000, 918000, 'Đã đóng'),
       (16, 1, '2025', 896000, 896000, 'Đã đóng'),
       (17, 1, '2025', 915000, 915000, 'Đã đóng'),
       (18, 1, '2025', 932000, 932000, 'Đã đóng'),
       (19, 1, '2025', 895000, 895000, 'Đã đóng'),
       (20, 1, '2025', 876000, 876000, 'Đã đóng'),
       (21, 1, '2025', 900000, 900000, 'Đã đóng'),
       (22, 1, '2025', 950000, 950000, 'Đã đóng'),
       (23, 1, '2025', 897000, 897000, 'Đã đóng'),
       (24, 1, '2025', 890000, 890000, 'Đã đóng'),
       (25, 1, '2025', 917000, 917000, 'Đã đóng'),
       (26, 1, '2025', 920000, 920000, 'Đã đóng'),
       (27, 1, '2025', 922000, 922000, 'Đã đóng'),
       (28, 1, '2025', 873000, 873000, 'Đã đóng'),
       (29, 1, '2025', 881000, 881000, 'Đã đóng'),
       (30, 1, '2025', 854000, 854000, 'Đã đóng'),
       (31, 1, '2025', 850000, 850000, 'Đã đóng'),
       (32, 1, '2025', 902000, 902000, 'Đã đóng'),
       (33, 1, '2025', 871000, 871000, 'Đã đóng'),
       (34, 1, '2025', 851000, 851000, 'Đã đóng'),
       (35, 1, '2025', 893000, 893000, 'Đã đóng'),
       (36, 1, '2025', 946000, 946000, 'Đã đóng'),
       (37, 1, '2025', 917000, 917000, 'Đã đóng'),
       (38, 1, '2025', 939000, 939000, 'Đã đóng'),
       (39, 1, '2025', 886000, 886000, 'Đã đóng'),
       (40, 1, '2025', 936000, 936000, 'Đã đóng'),
       (41, 1, '2025', 868000, 868000, 'Đã đóng'),
       (42, 1, '2025', 922000, 922000, 'Đã đóng'),
       (43, 1, '2025', 945000, 945000, 'Đã đóng'),
       (44, 1, '2025', 912000, 912000, 'Đã đóng'),
       (45, 1, '2025', 891000, 891000, 'Đã đóng'),
       (46, 1, '2025', 902000, 902000, 'Đã đóng'),
       (47, 1, '2025', 917000, 917000, 'Đã đóng'),
       (48, 1, '2025', 861000, 861000, 'Đã đóng'),
       (49, 1, '2025', 867000, 867000, 'Đã đóng'),
       (50, 1, '2025', 889000, 889000, 'Đã đóng'),
       (51, 1, '2025', 901000, 901000, 'Đã đóng'),
       (52, 1, '2025', 925000, 925000, 'Đã đóng'),
       (53, 1, '2025', 869000, 869000, 'Đã đóng'),
       (54, 1, '2025', 896000, 896000, 'Đã đóng'),
       (55, 1, '2025', 870000, 870000, 'Đã đóng'),
       (56, 1, '2025', 925000, 925000, 'Đã đóng'),
       (57, 1, '2025', 905000, 905000, 'Đã đóng'),
       (58, 1, '2025', 888000, 888000, 'Đã đóng'),
       (59, 1, '2025', 923000, 923000, 'Đã đóng'),
       (60, 1, '2025', 896000, 896000, 'Đã đóng'),
       (61, 1, '2025', 928000, 928000, 'Đã đóng'),
       (62, 1, '2025', 876000, 876000, 'Đã đóng'),
       (63, 1, '2025', 886000, 886000, 'Đã đóng'),
       (64, 1, '2025', 906000, 906000, 'Đã đóng'),
       (65, 1, '2025', 919000, 919000, 'Đã đóng'),
       (66, 1, '2025', 854000, 854000, 'Đã đóng'),
       (67, 1, '2025', 875000, 875000, 'Đã đóng'),
       (68, 1, '2025', 866000, 866000, 'Đã đóng'),
       (69, 1, '2025', 862000, 862000, 'Đã đóng'),
       (70, 1, '2025', 906000, 906000, 'Đã đóng'),
       (71, 1, '2025', 861000, 861000, 'Đã đóng'),
       (72, 1, '2025', 903000, 903000, 'Đã đóng'),
       (73, 1, '2025', 939000, 939000, 'Đã đóng'),
       (74, 1, '2025', 931000, 931000, 'Đã đóng'),
       (75, 1, '2025', 887000, 887000, 'Đã đóng'),
       (76, 1, '2025', 944000, 944000, 'Đã đóng'),
       (77, 1, '2025', 947000, 947000, 'Đã đóng'),
       (78, 1, '2025', 925000, 925000, 'Đã đóng'),
       (79, 1, '2025', 854000, 854000, 'Đã đóng'),
       (80, 1, '2025', 871000, 871000, 'Đã đóng'),
       (81, 1, '2025', 919000, 919000, 'Đã đóng'),
       (82, 1, '2025', 950000, 950000, 'Đã đóng'),
       (83, 1, '2025', 899000, 899000, 'Đã đóng'),
       (84, 1, '2025', 938000, 938000, 'Đã đóng'),
       (85, 1, '2025', 871000, 871000, 'Đã đóng'),
       (86, 1, '2025', 858000, 858000, 'Đã đóng'),
       (87, 1, '2025', 936000, 936000, 'Đã đóng'),
       (88, 1, '2025', 858000, 858000, 'Đã đóng'),
       (89, 1, '2025', 936000, 936000, 'Đã đóng'),
       (90, 1, '2025', 921000, 921000, 'Đã đóng'),
       (91, 1, '2025', 939000, 939000, 'Đã đóng'),
       (92, 1, '2025', 910000, 910000, 'Đã đóng'),
       (93, 1, '2025', 853000, 853000, 'Đã đóng'),
       (94, 1, '2025', 896000, 896000, 'Đã đóng'),
       (95, 1, '2025', 944000, 944000, 'Đã đóng'),
       (96, 1, '2025', 941000, 941000, 'Đã đóng'),
       (97, 1, '2025', 859000, 859000, 'Đã đóng'),
       (98, 1, '2025', 902000, 902000, 'Đã đóng'),
       (99, 1, '2025', 884000, 884000, 'Đã đóng'),
       (100, 1, '2025', 942000, 942000, 'Đã đóng');

-- 9
INSERT INTO KhoaHoc (MaCKDT, TenKhoaHoc, NienKhoaHoc)
VALUES (1, 'K21', '2021-2025'),
       (1, 'K22', '2022-2026'),
       (1, 'K23', '2023-2027'),

       (2, 'K24', '2024-2028'),
       (2, 'K25', '2025-2029');


-- 10
INSERT INTO ChuKyDaoTao (NamBatDau, NamKetThuc)
VALUES ('2020', '2024'),
       ('2024', '2028');

-- 11
-- Mỗi NHP có 10sv
INSERT INTO DangKy (MaNHP, MaSV)
VALUES (6, 1),
       (6, 2),
       (6, 3),
       (6, 4),
       (6, 5),
       (6, 6),
       (6, 7),
       (6, 8),
       (6, 9),
       (6, 10),
       (7, 11),
       (7, 12),
       (7, 13),
       (7, 14),
       (7, 15),
       (7, 16),
       (7, 17),
       (7, 18),
       (7, 19),
       (7, 20),
       (8, 21),
       (8, 22),
       (8, 23),
       (8, 24),
       (8, 25),
       (8, 26),
       (8, 27),
       (8, 28),
       (8, 29),
       (8, 30),
       (9, 31),
       (9, 32),
       (9, 33),
       (9, 34),
       (9, 35),
       (9, 36),
       (9, 37),
       (9, 38),
       (9, 39),
       (9, 40),
       (10, 41),
       (10, 42),
       (10, 43),
       (10, 44),
       (10, 45),
       (10, 46),
       (10, 47),
       (10, 48),
       (10, 49),
       (10, 50),
       (11, 51),
       (11, 52),
       (11, 53),
       (11, 54),
       (11, 55),
       (11, 56),
       (11, 57),
       (11, 58),
       (11, 59),
       (11, 60),
       (12, 61),
       (12, 62),
       (12, 63),
       (12, 64),
       (12, 65),
       (12, 66),
       (12, 67),
       (12, 68),
       (12, 69),
       (12, 70),
       (13, 71),
       (13, 72),
       (13, 73),
       (13, 74),
       (13, 75),
       (13, 76),
       (13, 77),
       (13, 78),
       (13, 79),
       (13, 80),
       (14, 81),
       (14, 82),
       (14, 83),
       (14, 84),
       (14, 85),
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
       (15, 96),
       (15, 97),
       (15, 98),
       (15, 99),
       (15, 100);


-- 12
INSERT INTO NhomHocPhan (MaGV, MaHP, MaLichDK, MaLop, HocKy, Nam, SiSo)
VALUES (2, 1, 3, 1, 1, 2025, 50),
       (2, 2, 3, 2, 1, 2025, 45),
       (2, 3, 3, 3, 1, 2025, 55),
       (2, 4, 3, 4, 1, 2025, 60),
       (2, 5, 3, 5, 1, 2025, 40),
       (2, 6, 3, 6, 1, 2025, 50),
       (2, 7, 3, 7, 1, 2025, 48),
       (2, 8, 3, 8, 1, 2025, 52),
       (2, 9, 3, 9, 1, 2025, 47),
       (2, 10, 3, 10, 1, 2025, 53),
       (2, 11, 3, 2, 1, 2025, 60),
       (2, 12, 3, 3, 1, 2025, 58),
       (2, 13, 3, 4, 1, 2025, 42),
       (2, 14, 3, 5, 1, 2025, 50),
       (2, 15, 3, 1, 1, 2025, 45);


-- 13
INSERT INTO LichHoc (MaLH, MaPH, MaNHP, Thu, TietBatDau, TuNgay, DenNgay, TietKetThuc, SoTiet)
VALUES (1, 1, 1, 'Thứ 2', 1, '2025-02-17', '2025-06-01', 3, 3),
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
       (15, 15, 15, 'Thứ 6', 4, '2025-02-17', '2025-06-01', 6, 3);


-- 14
INSERT INTO PhongHoc (TenPH, LoaiPH, CoSo, SucChua, TinhTrang)
VALUES ('A101', 'Lý thuyết', 'Cơ sở A', 60, 'Sẵn sàng'),
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
INSERT INTO HocPhan (MaHPTruoc, TenHP, SoTinChi, HeSoHocPhan, SoTietLyThuyet, SoTietThucHanh)
VALUES 
    (NULL, 'Lập trình hướng đối tượng', 3, '5:5', 30, 15),    -- Công nghệ phần mềm
    (NULL, 'Kỹ thuật vi điều khiển', 3, '5:5', 30, 15),       -- Hệ thống nhúng
    (NULL, 'Khai phá dữ liệu', 3, '5:5', 30, 15),             -- Khoa học dữ liệu
    (NULL, 'Phân tích dữ liệu với Python', 3, '5:5', 30, 15), -- Phân tích dữ liệu lớn
    (NULL, 'An toàn hệ thống mạng', 3, '5:5', 30, 15),        -- An ninh mạng
    (NULL, 'Quản lý ngân sách công', 3, '5:5', 45, 0),        -- Tài chính công
    (NULL, 'Thương mại quốc tế', 3, '5:5', 45, 0),            -- Kinh tế đối ngoại
    (NULL, 'Quản trị chiến lược', 3, '5:5', 45, 0),           -- Quản trị kinh doanh
    (NULL, 'Nguyên lý kế toán', 3, '5:5', 45, 0),             -- Kế toán kiểm toán
    (NULL, 'Nghiệp vụ ngân hàng', 3, '5:5', 45, 0),           -- Ngân hàng đầu tư

    (NULL, 'Triết học Mác – Lênin', 2, '6:4', 30, 0),
    (11, 'Kinh tế chính trị Mác – Lênin', 2, '6:4', 30, 0),
    (12, 'Chủ nghĩa xã hội khoa học', 2, '6:4', 30, 0),
    (13, 'Lịch sử Đảng Cộng sản Việt Nam', 2, '6:4', 30, 0),
    (14, 'Tư tưởng Hồ Chí Minh', 2, '6:4', 30, 0);



-- 17
INSERT INTO `KetQua` (`MaHP`, `MaSV`, `DiemThi`, `DiemHe4`, `DiemHe10`, `HocKy`, `Nam`)
VALUES (6, 1, 1.6, 0.0, 1.6, 1, '2025'),
       (6, 2, 5.2, 1.5, 5.2, 1, '2025'),
       (6, 3, 7.1, 3.0, 7.1, 1, '2025'),
       (6, 4, 5.3, 1.5, 5.3, 1, '2025'),
       (6, 5, 4.9, 1.0, 4.9, 1, '2025'),
       (6, 6, 9.1, 4.0, 9.1, 1, '2025'),
       (6, 7, 4.6, 1.0, 4.6, 1, '2025'),
       (6, 8, 6.8, 2.5, 6.8, 1, '2025'),
       (6, 9, 3.0, 0.0, 3.0, 1, '2025'),
       (6, 10, 6.4, 2.0, 6.4, 1, '2025'),
       (7, 11, 3.0, 0.0, 3.0, 1, '2025'),
       (7, 12, 8.7, 3.5, 8.7, 1, '2025'),
       (7, 13, 7.5, 3.0, 7.5, 1, '2025'),
       (7, 14, 4.6, 1.0, 4.6, 1, '2025'),
       (7, 15, 3.0, 0.0, 3.0, 1, '2025'),
       (7, 16, 1.1, 0.0, 1.1, 1, '2025'),
       (7, 17, 8.9, 3.5, 8.9, 1, '2025'),
       (7, 18, 6.1, 2.0, 6.1, 1, '2025'),
       (7, 19, 8.4, 3.5, 8.4, 1, '2025'),
       (7, 20, 1.3, 0.0, 1.3, 1, '2025'),
       (8, 21, 7.1, 3.0, 7.1, 1, '2025'),
       (8, 22, 1.9, 0.0, 1.9, 1, '2025'),
       (8, 23, 7.3, 3.0, 7.3, 1, '2025'),
       (8, 24, 4.0, 1.0, 4.0, 1, '2025'),
       (8, 25, 8.4, 3.5, 8.4, 1, '2025'),
       (8, 26, 4.9, 1.0, 4.9, 1, '2025'),
       (8, 27, 1.0, 0.0, 1.0, 1, '2025'),
       (8, 28, 8.9, 3.5, 8.9, 1, '2025'),
       (8, 29, 9.4, 4.0, 9.4, 1, '2025'),
       (8, 30, 4.4, 1.0, 4.4, 1, '2025'),
       (9, 31, 6.3, 2.0, 6.3, 1, '2025'),
       (9, 32, 1.4, 0.0, 1.4, 1, '2025'),
       (9, 33, 2.5, 0.0, 2.5, 1, '2025'),
       (9, 34, 3.2, 0.0, 3.2, 1, '2025'),
       (9, 35, 7.7, 3.0, 7.7, 1, '2025'),
       (9, 36, 1.5, 0.0, 1.5, 1, '2025'),
       (9, 37, 3.4, 0.0, 3.4, 1, '2025'),
       (9, 38, 6.4, 2.0, 6.4, 1, '2025'),
       (9, 39, 3.3, 0.0, 3.3, 1, '2025'),
       (9, 40, 7.4, 3.0, 7.4, 1, '2025'),
       (10, 41, 2.5, 0.0, 2.5, 1, '2025'),
       (10, 42, 0.8, 0.0, 0.8, 1, '2025'),
       (10, 43, 7.7, 3.0, 7.7, 1, '2025'),
       (10, 44, 2.1, 0.0, 2.1, 1, '2025'),
       (10, 45, 4.0, 1.0, 4.0, 1, '2025'),
       (10, 46, 6.5, 2.5, 6.5, 1, '2025'),
       (10, 47, 1.8, 0.0, 1.8, 1, '2025'),
       (10, 48, 1.6, 0.0, 1.6, 1, '2025'),
       (10, 49, 6.5, 2.5, 6.5, 1, '2025'),
       (10, 50, 9.2, 4.0, 9.2, 1, '2025'),
       (11, 51, 0.9, 0.0, 0.9, 1, '2025'),
       (11, 52, 5.9, 2.0, 5.9, 1, '2025'),
       (11, 53, 4.1, 1.0, 4.1, 1, '2025'),
       (11, 54, 4.5, 1.0, 4.5, 1, '2025'),
       (11, 55, 5.5, 2.0, 5.5, 1, '2025'),
       (11, 56, 4.6, 1.0, 4.6, 1, '2025'),
       (11, 57, 8.8, 3.5, 8.8, 1, '2025'),
       (11, 58, 8.2, 3.5, 8.2, 1, '2025'),
       (11, 59, 0.2, 0.0, 0.2, 1, '2025'),
       (11, 60, 0.2, 0.0, 0.2, 1, '2025'),
       (12, 61, 4.2, 1.0, 4.2, 1, '2025'),
       (12, 62, 1.4, 0.0, 1.4, 1, '2025'),
       (12, 63, 2.8, 0.0, 2.8, 1, '2025'),
       (12, 64, 10.0, 4.0, 10.0, 1, '2025'),
       (12, 65, 2.0, 0.0, 2.0, 1, '2025'),
       (12, 66, 6.5, 2.5, 6.5, 1, '2025'),
       (12, 67, 6.8, 2.5, 6.8, 1, '2025'),
       (12, 68, 5.7, 2.0, 5.7, 1, '2025'),
       (12, 69, 2.1, 0.0, 2.1, 1, '2025'),
       (12, 70, 1.8, 0.0, 1.8, 1, '2025'),
       (13, 71, 9.1, 4.0, 9.1, 1, '2025'),
       (13, 72, 3.2, 0.0, 3.2, 1, '2025'),
       (13, 73, 2.4, 0.0, 2.4, 1, '2025'),
       (13, 74, 8.2, 3.5, 8.2, 1, '2025'),
       (13, 75, 4.8, 1.0, 4.8, 1, '2025'),
       (13, 76, 3.3, 0.0, 3.3, 1, '2025'),
       (13, 77, 0.3, 0.0, 0.3, 1, '2025'),
       (13, 78, 4.7, 1.0, 4.7, 1, '2025'),
       (13, 79, 7.5, 3.0, 7.5, 1, '2025'),
       (13, 80, 0.1, 0.0, 0.1, 1, '2025'),
       (14, 81, 7.8, 3.0, 7.8, 1, '2025'),
       (14, 82, 0.3, 0.0, 0.3, 1, '2025'),
       (14, 83, 2.0, 0.0, 2.0, 1, '2025'),
       (14, 84, 7.0, 3.0, 7.0, 1, '2025'),
       (14, 85, 1.4, 0.0, 1.4, 1, '2025'),
       (14, 86, 4.1, 1.0, 4.1, 1, '2025'),
       (14, 87, 3.5, 0.0, 3.5, 1, '2025'),
       (14, 88, 0.0, 0.0, 0.0, 1, '2025'),
       (14, 89, 5.7, 2.0, 5.7, 1, '2025'),
       (14, 90, 8.8, 3.5, 8.8, 1, '2025'),
       (15, 91, 0.1, 0.0, 0.1, 1, '2025'),
       (15, 92, 9.6, 4.0, 9.6, 1, '2025'),
       (15, 93, 5.1, 1.5, 5.1, 1, '2025'),
       (15, 94, 6.3, 2.0, 6.3, 1, '2025'),
       (15, 95, 4.0, 1.0, 4.0, 1, '2025'),
       (15, 96, 8.9, 3.5, 8.9, 1, '2025'),
       (15, 97, 1.1, 0.0, 1.1, 1, '2025'),
       (15, 98, 3.8, 0.0, 3.8, 1, '2025'),
       (15, 99, 6.3, 2.0, 6.3, 1, '2025'),
       (15, 100, 8.0, 3.5, 8.0, 1, '2025');


-- 19
INSERT INTO `DiemQuaTrinh` (`MaKQ`, `DiemSo`)
VALUES (1, 5.6),
       (2, 6.4),
       (3, 3.4),
       (4, 2.8),
       (5, 9.3),
       (6, 7.7),
       (7, 1.9),
       (8, 4.2),
       (9, 8.5),
       (10, 0.7),
       (11, 6.1),
       (12, 2.3),
       (13, 5.0),
       (14, 9.8),
       (15, 4.5),
       (16, 1.2),
       (17, 7.4),
       (18, 3.6),
       (19, 8.9),
       (20, 0.4),
       (21, 5.7),
       (22, 2.1),
       (23, 6.8),
       (24, 9.1),
       (25, 4.0),
       (26, 1.5),
       (27, 7.2),
       (28, 3.3),
       (29, 8.6),
       (30, 0.9),
       (31, 5.4),
       (32, 2.7),
       (33, 6.5),
       (34, 9.4),
       (35, 4.3),
       (36, 1.0),
       (37, 7.6),
       (38, 3.8),
       (39, 8.7),
       (40, 0.2),
       (41, 5.9),
       (42, 2.5),
       (43, 6.9),
       (44, 9.2),
       (45, 4.7),
       (46, 1.4),
       (47, 7.8),
       (48, 3.1),
       (49, 8.8),
       (50, 0.5),
       (51, 5.2),
       (52, 2.9),
       (53, 6.3),
       (54, 9.5),
       (55, 4.1),
       (56, 1.6),
       (57, 7.3),
       (58, 3.5),
       (59, 8.4),
       (60, 0.8),
       (61, 5.5),
       (62, 2.4),
       (63, 6.6),
       (64, 9.7),
       (65, 4.4),
       (66, 1.3),
       (67, 7.5),
       (68, 3.7),
       (69, 8.3),
       (70, 0.6),
       (71, 5.8),
       (72, 2.6),
       (73, 6.7),
       (74, 9.6),
       (75, 4.6),
       (76, 1.7),
       (77, 7.9),
       (78, 3.2),
       (79, 8.2),
       (80, 0.3),
       (81, 5.3),
       (82, 2.2),
       (83, 6.2),
       (84, 9.9),
       (85, 4.8),
       (86, 1.8),
       (87, 7.1),
       (88, 3.9),
       (89, 8.1),
       (90, 0.1),
       (91, 5.1),
       (92, 2.0),
       (93, 6.0),
       (94, 9.0),
       (95, 4.9),
       (96, 1.1),
       (97, 7.0),
       (98, 3.0),
       (99, 8.0),
       (100, 10.0);


-- 20
INSERT INTO `CotDiem` (`MaDQT`, `TenCotDiem`, `DiemSo`, `HeSo`)
VALUES (1, 'Cột điểm 1', 5.6, 1),
       (2, 'Cột điểm 1', 6.4, 1),
       (3, 'Cột điểm 1', 3.4, 1),
       (4, 'Cột điểm 1', 2.8, 1),
       (5, 'Cột điểm 1', 9.3, 1),
       (6, 'Cột điểm 1', 7.7, 1),
       (7, 'Cột điểm 1', 1.9, 1),
       (8, 'Cột điểm 1', 4.2, 1),
       (9, 'Cột điểm 1', 8.5, 1),
       (10, 'Cột điểm 1', 0.7, 1),
       (11, 'Cột điểm 1', 6.1, 1),
       (12, 'Cột điểm 1', 2.3, 1),
       (13, 'Cột điểm 1', 5.0, 1),
       (14, 'Cột điểm 1', 9.8, 1),
       (15, 'Cột điểm 1', 4.5, 1),
       (16, 'Cột điểm 1', 1.2, 1),
       (17, 'Cột điểm 1', 7.4, 1),
       (18, 'Cột điểm 1', 3.6, 1),
       (19, 'Cột điểm 1', 8.9, 1),
       (20, 'Cột điểm 1', 0.4, 1),
       (21, 'Cột điểm 1', 5.7, 1),
       (22, 'Cột điểm 1', 2.1, 1),
       (23, 'Cột điểm 1', 6.8, 1),
       (24, 'Cột điểm 1', 9.1, 1),
       (25, 'Cột điểm 1', 4.0, 1),
       (26, 'Cột điểm 1', 1.5, 1),
       (27, 'Cột điểm 1', 7.2, 1),
       (28, 'Cột điểm 1', 3.3, 1),
       (29, 'Cột điểm 1', 8.6, 1),
       (30, 'Cột điểm 1', 0.9, 1),
       (31, 'Cột điểm 1', 5.4, 1),
       (32, 'Cột điểm 1', 2.7, 1),
       (33, 'Cột điểm 1', 6.5, 1),
       (34, 'Cột điểm 1', 9.4, 1),
       (35, 'Cột điểm 1', 4.3, 1),
       (36, 'Cột điểm 1', 1.0, 1),
       (37, 'Cột điểm 1', 7.6, 1),
       (38, 'Cột điểm 1', 3.8, 1),
       (39, 'Cột điểm 1', 8.7, 1),
       (40, 'Cột điểm 1', 0.2, 1),
       (41, 'Cột điểm 1', 5.9, 1),
       (42, 'Cột điểm 1', 2.5, 1),
       (43, 'Cột điểm 1', 6.9, 1),
       (44, 'Cột điểm 1', 9.2, 1),
       (45, 'Cột điểm 1', 4.7, 1),
       (46, 'Cột điểm 1', 1.4, 1),
       (47, 'Cột điểm 1', 7.8, 1),
       (48, 'Cột điểm 1', 3.1, 1),
       (49, 'Cột điểm 1', 8.8, 1),
       (50, 'Cột điểm 1', 0.5, 1),
       (51, 'Cột điểm 1', 5.2, 1),
       (52, 'Cột điểm 1', 2.9, 1),
       (53, 'Cột điểm 1', 6.3, 1),
       (54, 'Cột điểm 1', 9.5, 1),
       (55, 'Cột điểm 1', 4.1, 1),
       (56, 'Cột điểm 1', 1.6, 1),
       (57, 'Cột điểm 1', 7.3, 1),
       (58, 'Cột điểm 1', 3.5, 1),
       (59, 'Cột điểm 1', 8.4, 1),
       (60, 'Cột điểm 1', 0.8, 1),
       (61, 'Cột điểm 1', 5.5, 1),
       (62, 'Cột điểm 1', 2.4, 1),
       (63, 'Cột điểm 1', 6.6, 1),
       (64, 'Cột điểm 1', 9.7, 1),
       (65, 'Cột điểm 1', 4.4, 1),
       (66, 'Cột điểm 1', 1.3, 1),
       (67, 'Cột điểm 1', 7.5, 1),
       (68, 'Cột điểm 1', 3.7, 1),
       (69, 'Cột điểm 1', 8.3, 1),
       (70, 'Cột điểm 1', 0.6, 1),
       (71, 'Cột điểm 1', 5.8, 1),
       (72, 'Cột điểm 1', 2.6, 1),
       (73, 'Cột điểm 1', 6.7, 1),
       (74, 'Cột điểm 1', 9.6, 1),
       (75, 'Cột điểm 1', 4.6, 1),
       (76, 'Cột điểm 1', 1.7, 1),
       (77, 'Cột điểm 1', 7.9, 1),
       (78, 'Cột điểm 1', 3.2, 1),
       (79, 'Cột điểm 1', 8.2, 1),
       (80, 'Cột điểm 1', 0.3, 1),
       (81, 'Cột điểm 1', 5.3, 1),
       (82, 'Cột điểm 1', 2.2, 1),
       (83, 'Cột điểm 1', 6.2, 1),
       (84, 'Cột điểm 1', 9.9, 1),
       (85, 'Cột điểm 1', 4.8, 1),
       (86, 'Cột điểm 1', 1.8, 1),
       (87, 'Cột điểm 1', 7.1, 1),
       (88, 'Cột điểm 1', 3.9, 1),
       (89, 'Cột điểm 1', 8.1, 1),
       (90, 'Cột điểm 1', 0.1, 1),
       (91, 'Cột điểm 1', 5.1, 1),
       (92, 'Cột điểm 1', 2.0, 1),
       (93, 'Cột điểm 1', 6.0, 1),
       (94, 'Cột điểm 1', 9.0, 1),
       (95, 'Cột điểm 1', 4.9, 1),
       (96, 'Cột điểm 1', 1.1, 1),
       (97, 'Cột điểm 1', 7.0, 1),
       (98, 'Cột điểm 1', 3.0, 1),
       (99, 'Cột điểm 1', 8.0, 1),
       (100, 'Cột điểm 1', 10.0, 1);

INSERT INTO `CotDiem` (`MaDQT`, `TenCotDiem`, `DiemSo`, `HeSo`)
VALUES (1, 'Cột điểm 2', 5.6, 1),
       (2, 'Cột điểm 2', 6.4, 1),
       (3, 'Cột điểm 2', 3.4, 1),
       (4, 'Cột điểm 2', 2.8, 1),
       (5, 'Cột điểm 2', 9.3, 1),
       (6, 'Cột điểm 2', 7.7, 1),
       (7, 'Cột điểm 2', 1.9, 1),
       (8, 'Cột điểm 2', 4.2, 1),
       (9, 'Cột điểm 2', 8.5, 1),
       (10, 'Cột điểm 2', 0.7, 1),
       (11, 'Cột điểm 2', 6.1, 1),
       (12, 'Cột điểm 2', 2.3, 1),
       (13, 'Cột điểm 2', 5.0, 1),
       (14, 'Cột điểm 2', 9.8, 1),
       (15, 'Cột điểm 2', 4.5, 1),
       (16, 'Cột điểm 2', 1.2, 1),
       (17, 'Cột điểm 2', 7.4, 1),
       (18, 'Cột điểm 2', 3.6, 1),
       (19, 'Cột điểm 2', 8.9, 1),
       (20, 'Cột điểm 2', 0.4, 1),
       (21, 'Cột điểm 2', 5.7, 1),
       (22, 'Cột điểm 2', 2.1, 1),
       (23, 'Cột điểm 2', 6.8, 1),
       (24, 'Cột điểm 2', 9.1, 1),
       (25, 'Cột điểm 2', 4.0, 1),
       (26, 'Cột điểm 2', 1.5, 1),
       (27, 'Cột điểm 2', 7.2, 1),
       (28, 'Cột điểm 2', 3.3, 1),
       (29, 'Cột điểm 2', 8.6, 1),
       (30, 'Cột điểm 2', 0.9, 1),
       (31, 'Cột điểm 2', 5.4, 1),
       (32, 'Cột điểm 2', 2.7, 1),
       (33, 'Cột điểm 2', 6.5, 1),
       (34, 'Cột điểm 2', 9.4, 1),
       (35, 'Cột điểm 2', 4.3, 1),
       (36, 'Cột điểm 2', 1.0, 1),
       (37, 'Cột điểm 2', 7.6, 1),
       (38, 'Cột điểm 2', 3.8, 1),
       (39, 'Cột điểm 2', 8.7, 1),
       (40, 'Cột điểm 2', 0.2, 1),
       (41, 'Cột điểm 2', 5.9, 1),
       (42, 'Cột điểm 2', 2.5, 1),
       (43, 'Cột điểm 2', 6.9, 1),
       (44, 'Cột điểm 2', 9.2, 1),
       (45, 'Cột điểm 2', 4.7, 1),
       (46, 'Cột điểm 2', 1.4, 1),
       (47, 'Cột điểm 2', 7.8, 1),
       (48, 'Cột điểm 2', 3.1, 1),
       (49, 'Cột điểm 2', 8.8, 1),
       (50, 'Cột điểm 2', 0.5, 1),
       (51, 'Cột điểm 2', 5.2, 1),
       (52, 'Cột điểm 2', 2.9, 1),
       (53, 'Cột điểm 2', 6.3, 1),
       (54, 'Cột điểm 2', 9.5, 1),
       (55, 'Cột điểm 2', 4.1, 1),
       (56, 'Cột điểm 2', 1.6, 1),
       (57, 'Cột điểm 2', 7.3, 1),
       (58, 'Cột điểm 2', 3.5, 1),
       (59, 'Cột điểm 2', 8.4, 1),
       (60, 'Cột điểm 2', 0.8, 1),
       (61, 'Cột điểm 2', 5.5, 1),
       (62, 'Cột điểm 2', 2.4, 1),
       (63, 'Cột điểm 2', 6.6, 1),
       (64, 'Cột điểm 2', 9.7, 1),
       (65, 'Cột điểm 2', 4.4, 1),
       (66, 'Cột điểm 2', 1.3, 1),
       (67, 'Cột điểm 2', 7.5, 1),
       (68, 'Cột điểm 2', 3.7, 1),
       (69, 'Cột điểm 2', 8.3, 1),
       (70, 'Cột điểm 2', 0.6, 1),
       (71, 'Cột điểm 2', 5.8, 1),
       (72, 'Cột điểm 2', 2.6, 1),
       (73, 'Cột điểm 2', 6.7, 1),
       (74, 'Cột điểm 2', 9.6, 1),
       (75, 'Cột điểm 2', 4.6, 1),
       (76, 'Cột điểm 2', 1.7, 1),
       (77, 'Cột điểm 2', 7.9, 1),
       (78, 'Cột điểm 2', 3.2, 1),
       (79, 'Cột điểm 2', 8.2, 1),
       (80, 'Cột điểm 2', 0.3, 1),
       (81, 'Cột điểm 2', 5.3, 1),
       (82, 'Cột điểm 2', 2.2, 1),
       (83, 'Cột điểm 2', 6.2, 1),
       (84, 'Cột điểm 2', 9.9, 1),
       (85, 'Cột điểm 2', 4.8, 1),
       (86, 'Cột điểm 2', 1.8, 1),
       (87, 'Cột điểm 2', 7.1, 1),
       (88, 'Cột điểm 2', 3.9, 1),
       (89, 'Cột điểm 2', 8.1, 1),
       (90, 'Cột điểm 2', 0.1, 1),
       (91, 'Cột điểm 2', 5.1, 1),
       (92, 'Cột điểm 2', 2.0, 1),
       (93, 'Cột điểm 2', 6.0, 1),
       (94, 'Cột điểm 2', 9.0, 1),
       (95, 'Cột điểm 2', 4.9, 1),
       (96, 'Cột điểm 2', 1.1, 1),
       (97, 'Cột điểm 2', 7.0, 1),
       (98, 'Cột điểm 2', 3.0, 1),
       (99, 'Cột điểm 2', 8.0, 1),
       (100, 'Cột điểm 2', 10.0, 1);



-- 21
-- tổng tiền đang để tạm, chưa tính chính xác theo ngành của sinh viên và số tín chỉ của học phần
INSERT INTO HocPhiHocPhan (MaSV, MaHP, TongTien, HocKy, Nam)
VALUES (1, 6, 871000, 1, '2025'),
       (2, 6, 892000, 1, '2025'),
       (3, 6, 896000, 1, '2025'),
       (4, 6, 869000, 1, '2025'),
       (5, 6, 869000, 1, '2025'),
       (6, 6, 879000, 1, '2025'),
       (7, 6, 933000, 1, '2025'),
       (8, 6, 874000, 1, '2025'),
       (9, 6, 859000, 1, '2025'),
       (10, 6, 854000, 1, '2025'),
       (11, 7, 900000, 1, '2025'),
       (12, 7, 941000, 1, '2025'),
       (13, 7, 909000, 1, '2025'),
       (14, 7, 855000, 1, '2025'),
       (15, 7, 918000, 1, '2025'),
       (16, 7, 896000, 1, '2025'),
       (17, 7, 915000, 1, '2025'),
       (18, 7, 932000, 1, '2025'),
       (19, 7, 895000, 1, '2025'),
       (20, 7, 876000, 1, '2025'),
       (21, 8, 900000, 1, '2025'),
       (22, 8, 950000, 1, '2025'),
       (23, 8, 897000, 1, '2025'),
       (24, 8, 890000, 1, '2025'),
       (25, 8, 917000, 1, '2025'),
       (26, 8, 920000, 1, '2025'),
       (27, 8, 922000, 1, '2025'),
       (28, 8, 873000, 1, '2025'),
       (29, 8, 881000, 1, '2025'),
       (30, 8, 854000, 1, '2025'),
       (31, 9, 850000, 1, '2025'),
       (32, 9, 902000, 1, '2025'),
       (33, 9, 871000, 1, '2025'),
       (34, 9, 851000, 1, '2025'),
       (35, 9, 893000, 1, '2025'),
       (36, 9, 946000, 1, '2025'),
       (37, 9, 917000, 1, '2025'),
       (38, 9, 939000, 1, '2025'),
       (39, 9, 886000, 1, '2025'),
       (40, 9, 936000, 1, '2025'),
       (41, 10, 868000, 1, '2025'),
       (42, 10, 922000, 1, '2025'),
       (43, 10, 945000, 1, '2025'),
       (44, 10, 912000, 1, '2025'),
       (45, 10, 891000, 1, '2025'),
       (46, 10, 902000, 1, '2025'),
       (47, 10, 917000, 1, '2025'),
       (48, 10, 861000, 1, '2025'),
       (49, 10, 867000, 1, '2025'),
       (50, 10, 889000, 1, '2025'),
       (51, 11, 901000, 1, '2025'),
       (52, 11, 925000, 1, '2025'),
       (53, 11, 869000, 1, '2025'),
       (54, 11, 896000, 1, '2025'),
       (55, 11, 870000, 1, '2025'),
       (56, 11, 925000, 1, '2025'),
       (57, 11, 905000, 1, '2025'),
       (58, 11, 888000, 1, '2025'),
       (59, 11, 923000, 1, '2025'),
       (60, 11, 896000, 1, '2025'),
       (61, 12, 928000, 1, '2025'),
       (62, 12, 876000, 1, '2025'),
       (63, 12, 886000, 1, '2025'),
       (64, 12, 906000, 1, '2025'),
       (65, 12, 919000, 1, '2025'),
       (66, 12, 854000, 1, '2025'),
       (67, 12, 875000, 1, '2025'),
       (68, 12, 866000, 1, '2025'),
       (69, 12, 862000, 1, '2025'),
       (70, 12, 906000, 1, '2025'),
       (71, 13, 861000, 1, '2025'),
       (72, 13, 903000, 1, '2025'),
       (73, 13, 939000, 1, '2025'),
       (74, 13, 931000, 1, '2025'),
       (75, 13, 887000, 1, '2025'),
       (76, 13, 944000, 1, '2025'),
       (77, 13, 947000, 1, '2025'),
       (78, 13, 925000, 1, '2025'),
       (79, 13, 854000, 1, '2025'),
       (80, 13, 871000, 1, '2025'),
       (81, 14, 919000, 1, '2025'),
       (82, 14, 950000, 1, '2025'),
       (83, 14, 899000, 1, '2025'),
       (84, 14, 938000, 1, '2025'),
       (85, 14, 871000, 1, '2025'),
       (86, 14, 858000, 1, '2025'),
       (87, 14, 936000, 1, '2025'),
       (88, 14, 858000, 1, '2025'),
       (89, 14, 936000, 1, '2025'),
       (90, 14, 921000, 1, '2025'),
       (91, 15, 939000, 1, '2025'),
       (92, 15, 910000, 1, '2025'),
       (93, 15, 853000, 1, '2025'),
       (94, 15, 896000, 1, '2025'),
       (95, 15, 944000, 1, '2025'),
       (96, 15, 941000, 1, '2025'),
       (97, 15, 859000, 1, '2025'),
       (98, 15, 902000, 1, '2025'),
       (99, 15, 884000, 1, '2025'),
       (100, 15, 942000, 1, '2025');


-- 22
INSERT INTO `CaThi` (`MaHP`, `MaPH`, `Thu`, `ThoiGianBatDau`, `ThoiLuong`, `HocKy`, `Nam`, `Status`)
VALUES (6, 1, 'Thứ 2', '15/05/2025 07:30:00', '01:30', 1, '2025', 1),
       (7, 2, 'Thứ 3', '16/05/2025 09:00:00', '01:30', 1, '2025', 1),
       (8, 3, 'Thứ 4', '17/05/2025 13:00:00', '02:00', 1, '2025', 1),
       (9, 4, 'Thứ 5', '18/05/2025 15:00:00', '01:45', 1, '2025', 1),
       (10, 5, 'Thứ 6', '19/05/2025 08:00:00', '01:30', 1, '2025', 1),
       (11, 6, 'Thứ 2', '10/10/2025 07:30:00', '01:30', 1, '2025', 1),
       (12, 7, 'Thứ 3', '11/10/2025 09:00:00', '01:30', 1, '2025', 1),
       (13, 8, 'Thứ 4', '12/10/2025 13:00:00', '02:00', 1, '2025', 1),
       (14, 9, 'Thứ 5', '13/10/2025 15:00:00', '01:45', 1, '2025', 1),
       (15, 10, 'Thứ 6', '14/10/2025 08:00:00', '01:30', 1, '2025', 1);

-- 23
INSERT INTO `CaThi_SinhVien` (`MaCT`, `MaSV`)
VALUES (1, 1),
       (1, 2),
       (1, 3),
       (1, 4),
       (1, 5),
       (1, 6),
       (1, 7),
       (1, 8),
       (1, 9),
       (1, 10),
       (2, 11),
       (2, 12),
       (2, 13),
       (2, 14),
       (2, 15),
       (2, 16),
       (2, 17),
       (2, 18),
       (2, 19),
       (2, 20),
       (3, 21),
       (3, 22),
       (3, 23),
       (3, 24),
       (3, 25),
       (3, 26),
       (3, 27),
       (3, 28),
       (3, 29),
       (3, 30),
       (4, 31),
       (4, 32),
       (4, 33),
       (4, 34),
       (4, 35),
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
       (5, 46),
       (5, 47),
       (5, 48),
       (5, 49),
       (5, 50),
       (6, 51),
       (6, 52),
       (6, 53),
       (6, 54),
       (6, 55),
       (6, 56),
       (6, 57),
       (6, 58),
       (6, 59),
       (6, 60),
       (7, 61),
       (7, 62),
       (7, 63),
       (7, 64),
       (7, 65),
       (7, 66),
       (7, 67),
       (7, 68),
       (7, 69),
       (7, 70),
       (8, 71),
       (8, 72),
       (8, 73),
       (8, 74),
       (8, 75),
       (8, 76),
       (8, 77),
       (8, 78),
       (8, 79),
       (8, 80),
       (9, 81),
       (9, 82),
       (9, 83),
       (9, 84),
       (9, 85),
       (9, 86),
       (9, 87),
       (9, 88),
       (9, 89),
       (9, 90),
       (10, 91),
       (10, 92),
       (10, 93),
       (10, 94),
       (10, 95),
       (10, 96),
       (10, 97),
       (10, 98),
       (10, 99),
       (10, 100);


-- 24
INSERT INTO ChuongTrinhDaoTao_HocPhan (MaCTDT, MaHP)
VALUES
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
INSERT INTO ChuongTrinhDaoTao (MaCKDT, MaNganh, LoaiHinhDT, TrinhDo)
VALUES (1, 1, 'Chính quy', 'Cử nhân'),
       (1, 2, 'Chính quy', 'Cử nhân'),
       (2, 1, 'Chính quy', 'Cử nhân'),
       (2, 2, 'Chính quy', 'Cử nhân');


-- 26
-- tài khoản quản trị viên
INSERT INTO TaiKhoan (MaNQ, TenDangNhap, MatKhau)
VALUES (2, 'admin', '$2a$11$MVrjcR5/HRmUmTiWNV2uX.6uGJyg/hwl.d9Mcr.g.M5N133J0Timu'),
       (3, 'an', '$2a$11$YZQfbakMQk1uUZ0ojbU.gOg5zrd7ncbMcz4402rhq4nc4PjDYrtMK');
-- tài khoản sinh viên
INSERT INTO TaiKhoan (MaNQ, TenDangNhap, MatKhau, Type)
VALUES (1, 'sinhvien', '$2a$11$YZQfbakMQk1uUZ0ojbU.gOg5zrd7ncbMcz4402rhq4nc4PjDYrtMK', 'Sinh viên');

-- tài khoản test giảng viên
INSERT INTO TaiKhoan (MaNQ, TenDangNhap, MatKhau)
VALUES (4, 'giangvien', '$2a$11$YZQfbakMQk1uUZ0ojbU.gOg5zrd7ncbMcz4402rhq4nc4PjDYrtMK');

-- 27
INSERT INTO NhomQuyen (TenNhomQuyen)
VALUES ('SinhVien'),
       ('Admin'),
       ('AnQuyen'),
       ('GiangVien');

-- 28
INSERT INTO ChiTietQuyen (MaCN, MaNQ, HanhDong)
VALUES (1, 2, 'Xem'),
       (1, 2, 'Them'),
       (1, 2, 'Sua'),
       (1, 2, 'Xoa'),
       (2, 2, 'Xem'),
       (2, 2, 'Them'),
       (2, 2, 'Sua'),
       (2, 2, 'Xoa'),
       (3, 2, 'Xem'),
       (3, 2, 'Them'),
       (3, 2, 'Sua'),
       (3, 2, 'Xoa'),
       (4, 2, 'Xem'),
       (4, 2, 'Them'),
       (4, 2, 'Sua'),
       (4, 2, 'Xoa'),
       (5, 2, 'Xem'),
       (5, 2, 'Them'),
       (5, 2, 'Sua'),
       (5, 2, 'Xoa'),
       (6, 2, 'Xem'),
       (6, 2, 'Them'),
       (6, 2, 'Sua'),
       (6, 2, 'Xoa'),
       (7, 2, 'Xem'),
       (7, 2, 'Them'),
       (7, 2, 'Sua'),
       (7, 2, 'Xoa'),
       (8, 2, 'Xem'),
       (8, 2, 'Them'),
       (8, 2, 'Sua'),
       (8, 2, 'Xoa'),
       (9, 2, 'Xem'),
       (9, 2, 'Them'),
       (9, 2, 'Sua'),
       (9, 2, 'Xoa'),
       (10, 2, 'Xem'),
       (10, 2, 'Them'),
       (10, 2, 'Sua'),
       (10, 2, 'Xoa'),
       (11, 2, 'Xem'),
       (11, 2, 'Them'),
       (12, 2, 'Xem'),
       (12, 2, 'Them'),
       (13, 2, 'Xem'),
       (13, 2, 'Them'),
       (14, 2, 'Xem'),
       (14, 2, 'Them'),
       (15, 2, 'Xem'),
       (15, 2, 'Them'),
       (15, 2, 'Sua'),
       (15, 2, 'Xoa'),
       (16, 2, 'Xem'),
       (16, 2, 'Them'),
       (16, 2, 'Sua'),
       (16, 2, 'Xoa'),
       (17, 2, 'Xem'),
       (17, 2, 'Them'),
       (17, 2, 'Sua'),
       (17, 2, 'Xoa'),
       (18, 2, 'Xem'),


       (1, 3, 'Xem'),
       (1, 3, 'Them'),
       (1, 3, 'Sua'),
       (1, 3, 'Xoa'),
       (2, 3, 'Xem'),
       (2, 3, 'Them'),
       (2, 3, 'Sua'),
       (2, 3, 'Xoa'),

       (12, 4, 'Xem'),
       (12, 4, 'Them');

-- 29
INSERT INTO ChucNang (TenChucNang)
VALUES ('SINHVIEN'),
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
       ('NHAPDIEMTHI'),
       ('HOCPHI'),
       ('MODANGKYHOCPHAN'),
       ('TAIKHOAN'),
       ('PHANQUYEN'),
       ('THONGKE');

-- 30
INSERT INTO LichDangKy (ThoiGianBatDau, ThoiGianKetThuc, Status)
VALUES ('1990-01-01 08:00:00', '1990-01-03 08:00:00', 1), -- Lịch ở năm 1990
       ('2024-12-15 10:00:00', '2024-12-17 10:00:00', 1), -- Lịch cuối năm 2024
       ('2025-06-05 09:00:00', '2025-06-07 09:00:00', 1);
-- Lịch giữa năm 2025


-- TKSV

INSERT INTO `TaiKhoan` (`MaNQ`, `TenDangNhap`, `MatKhau`, `Type`)
VALUES (1, 'sv1', '123456', 'Sinh viên'),
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
    ADD CONSTRAINT SinhVien_Lop FOREIGN KEY (MaLop) REFERENCES `Lop` (MaLop);

ALTER TABLE `Lop`
    ADD CONSTRAINT Lop_GiangVien FOREIGN KEY (MaGV) REFERENCES `GiangVien` (MaGV);

ALTER TABLE `Lop`
    ADD CONSTRAINT Lop_Nganh FOREIGN KEY (MaNganh) REFERENCES `Nganh` (MaNganh);

ALTER TABLE `Nganh`
    ADD CONSTRAINT `Nganh_Khoa` FOREIGN KEY (MaKhoa) REFERENCES `Khoa` (MaKhoa);

ALTER TABLE `GiangVien`
    ADD CONSTRAINT `GiangVien_Khoa` FOREIGN KEY (MaKhoa) REFERENCES `Khoa` (MaKhoa);

ALTER TABLE `ChuyenNganh`
    ADD CONSTRAINT `ChuyenNganh_Nganh` FOREIGN KEY (MaNganh) REFERENCES `Nganh` (MaNganh);

ALTER TABLE `HocPhiTinChi`
    ADD CONSTRAINT `HocPhiTinChi_Nganh` FOREIGN KEY (MaNganh) REFERENCES `Nganh` (MaNganh);

ALTER TABLE `HocPhiSV`
    ADD CONSTRAINT `HocPhiSV_SinhVien` FOREIGN KEY (MaSV) REFERENCES `SinhVien` (MaSV);

ALTER TABLE `SinhVien`
    ADD CONSTRAINT `SinhVien_KhoaHoc` FOREIGN KEY (MaKhoaHoc) REFERENCES `KhoaHoc` (MaKhoaHoc);

ALTER TABLE `KhoaHoc`
    ADD CONSTRAINT `KhoaHoc_ChuKyDaoTao` FOREIGN KEY (MaCKDT) REFERENCES `ChuKyDaoTao` (MaCKDT);

ALTER TABLE `DangKy`
    ADD CONSTRAINT `DangKy_SinhVien` FOREIGN KEY (MaSV) REFERENCES `SinhVien` (MaSV),
    ADD CONSTRAINT `DangKy_NhomHocPhan` FOREIGN KEY (MaNHP) REFERENCES `NhomHocPhan`(MaNHP);


ALTER TABLE `LichHoc`
    ADD CONSTRAINT `LichHoc_NhomHocPhan` FOREIGN KEY (MaNHP) REFERENCES `NhomHocPhan` (MaNHP),
    ADD CONSTRAINT `LichHoc_PhongHoc` FOREIGN KEY (MaPH) REFERENCES `PhongHoc`(MaPH);


ALTER TABLE `NhomHocPhan`
    ADD CONSTRAINT `NhomHocPhan_HocPhan` FOREIGN KEY (MaHP) REFERENCES `HocPhan` (MaHP);

ALTER TABLE `CaThi`
    ADD CONSTRAINT `CaThi_PhongHoc` FOREIGN KEY (MaPH) REFERENCES `PhongHoc` (MaPH),
    ADD CONSTRAINT `CaThi_HocPhan` FOREIGN KEY (MaHP) REFERENCES `HocPhan`(MaHP);

ALTER TABLE `CaThi_SinhVien`
    ADD CONSTRAINT `CaThi_SinhVien_CaThi` FOREIGN KEY (MaCT) REFERENCES `CaThi` (MaCT),
    ADD CONSTRAINT `CaThi_SinhVien_SinhVien` FOREIGN KEY (MaSV) REFERENCES `SinhVien`(MaSV);



ALTER TABLE `HocPhiHocPhan`
    ADD CONSTRAINT `HocPhiHocPhan_HocPhan` FOREIGN KEY (MaHP) REFERENCES `HocPhan` (MaHP),
    ADD CONSTRAINT `HocPhiHocPhan_SinhVien` FOREIGN KEY (MaSV) REFERENCES `SinhVien`(MaSV);



ALTER TABLE `DiemQuaTrinh`
    ADD CONSTRAINT `DiemQuaTrinh_KetQua` FOREIGN KEY (MaKQ) REFERENCES `KetQua` (MaKQ);

ALTER TABLE `KetQua`
    ADD CONSTRAINT `KetQua_HocPhan` FOREIGN KEY (MaHP) REFERENCES `HocPhan` (MaHP);

ALTER TABLE `KetQua`
    ADD CONSTRAINT `KetQua_SinhVien` FOREIGN KEY (MaSV) REFERENCES `SinhVien` (MaSV);

ALTER TABLE `CotDiem`
    ADD CONSTRAINT `CotDiem_DiemQuaTrinh` FOREIGN KEY (MaDQT) REFERENCES `DiemQuaTrinh` (MaDQT);



ALTER TABLE `SinhVien`
    ADD CONSTRAINT `SinhVien_TaiKhoan` FOREIGN KEY (MaTK) REFERENCES `TaiKhoan` (MaTK);

ALTER TABLE `GiangVien`
    ADD CONSTRAINT `GiangVien_TaiKhoan` FOREIGN KEY (MaTK) REFERENCES `TaiKhoan` (MaTK);

ALTER TABLE `TaiKhoan`
    ADD CONSTRAINT `TaiKhoan_NhomQuyen` FOREIGN KEY (MaNQ) REFERENCES `NhomQuyen` (MaNQ);

ALTER TABLE `ChiTietQuyen`
    ADD CONSTRAINT `ChiTietQuyen_NhomQuyen` FOREIGN KEY (MaNQ) REFERENCES `NhomQuyen` (MaNQ),
    ADD CONSTRAINT `ChiTietQuyen_ChucNang` FOREIGN KEY (MaCN) REFERENCES `ChucNang`(MaCN);

ALTER TABLE `NhomHocPhan`
    ADD CONSTRAINT `NhomHocPhan_GiangVien` FOREIGN KEY (MaGV) REFERENCES `GiangVien` (MaGV);

ALTER TABLE `ChuongTrinhDaoTao_HocPhan`
    ADD CONSTRAINT `ChuongTrinhDaoTao_HocPhan_HocPhan` FOREIGN KEY (MaHP) REFERENCES `HocPhan` (MaHP),
    ADD CONSTRAINT `ChuongTrinhDaoTao_HocPhan_ChuongTrinhDaoTao` FOREIGN KEY (MaCTDT) REFERENCES `ChuongTrinhDaoTao`(MaCTDT);

ALTER TABLE `ChuongTrinhDaoTao`
    ADD CONSTRAINT `ChuongTrinhDaoTao_ChuKyDaoTao` FOREIGN KEY (MaCKDT) REFERENCES `ChuKyDaoTao` (MaCKDT),
    ADD CONSTRAINT `ChuongTrinhDaoTao_Nganh` FOREIGN KEY (MaNganh) REFERENCES `Nganh`(MaNganh);

ALTER TABLE `NhomHocPhan`
    ADD CONSTRAINT `NhomHocPhan_LichDangky` FOREIGN KEY (MaLichDK) REFERENCES `LichDangky` (MaLichDK);

ALTER TABLE `NhomHocPhan`
    ADD CONSTRAINT `NhomHocPhan_Lop` FOREIGN KEY (MaLop) REFERENCES `Lop` (MaLop);

SELECT *
FROM SinhVien;
SELECT *
FROM Nganh;
SELECT *
FROM Lop;























