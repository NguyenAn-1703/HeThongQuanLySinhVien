-- test tốt nghiệp
-- Sinh viên có mã 1 cần học học phần từ 6 đến 15 để tốt nghiệp

-- xóa kết quả trước đó của sv có mã 1
DELETE CD
FROM `CotDiem` AS CD 
JOIN `DiemQuaTrinh` AS DQT ON CD.MaDQT = DQT.MaDQT
JOIN `KetQua` AS KQ ON DQT.MaKQ = KQ.MaKQ 
WHERE KQ.MaSV = 1;

DELETE DQT
FROM `DiemQuaTrinh` AS DQT
JOIN `KetQua` AS KQ ON DQT.MaKQ = KQ.MaKQ 
WHERE KQ.MaSV = 1;

DELETE
FROM `KetQua` 
WHERE MaSV = 1;

INSERT INTO `KetQua` (`MaHP`, `MaSV`, `DiemThi`, `HocKy`, `Nam`)
VALUES
(6, 1, 8.6, 2, '2024'),
(7, 1, 8.6, 2, '2024'),
(8, 1, 8.8, 4, '2024'),
(9, 1, 9.0, 4, '2024'),
(10, 1, 9.2, 4, '2024'),
(11, 1, 8.9, 4, '2024'),
(12, 1, 9.3, 4, '2024'),
(13, 1, 9.1, 4, '2024'),
(14, 1, 8.7, 4, '2024'),
(15, 1, 9.4, 4, '2024');

INSERT INTO `DiemQuaTrinh` (`MaKQ`, `DiemSo`)
SELECT MaKQ, 10
FROM KetQua
WHERE MaSV = 1
ORDER BY MaKQ ASC;


