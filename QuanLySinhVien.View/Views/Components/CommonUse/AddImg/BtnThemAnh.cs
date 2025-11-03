namespace QuanLySinhVien.View.Views.Components.CommonUse.AddImg;

public class BtnThemAnh : TitleButton
{
    public BtnThemAnh(string content) : base(content)
    {
        _mouseDown += () => OnMouseDown();
    }

    public event Action<string> OnClickAddImg;

    private void OnMouseDown()
    {
        using (var openFile = new OpenFileDialog())
        {
            openFile.Title = "Chọn ảnh";
            openFile.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                // đường dẫn ảnh người dùng chọn
                var selectedPath = openFile.FileName;
                // Tạo thư mục lưu ảnh trong project (nếu chưa có)
                var imageFolder = Path.Combine(Application.StartupPath, "img/portrait");

                if (!Directory.Exists(imageFolder))
                    Directory.CreateDirectory(imageFolder);

                // Tạo tên file đích (đảm bảo không trùng)
                var fileName = Path.GetFileName(selectedPath);
                var destPath = Path.Combine(imageFolder, fileName);

                // Nếu file trùng tên, đổi tên mới
                var count = 1;
                while (File.Exists(destPath))
                {
                    var nameOnly = Path.GetFileNameWithoutExtension(fileName);
                    var ext = Path.GetExtension(fileName);
                    destPath = Path.Combine(imageFolder, $"{nameOnly}_{count}{ext}");
                    count++;
                }

                // Sao chép ảnh vào thư mục project
                File.Copy(selectedPath, destPath);

                var finalFileName = Path.GetFileName(destPath);

                var relativePath = Path.Combine("img", "portrait", finalFileName).Replace("\\", "/");
                Console.WriteLine(relativePath);
                OnClickAddImg?.Invoke(relativePath);
            }
        }
    }
}