namespace QuanLySinhVien.Views.Components.CommonUse.AddImg;

public class BtnThemAnh : TitleButton
{
    public event Action<string> OnClickAddImg;
    
    public BtnThemAnh(string content) : base (content)
    {
        _mouseDown += () => OnMouseDown();
    }

    void OnMouseDown()
    {
        using (OpenFileDialog openFile = new OpenFileDialog())
        {
            openFile.Title = "Chọn ảnh";
            openFile.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                // đường dẫn ảnh người dùng chọn
                string selectedPath = openFile.FileName;
                // Tạo thư mục lưu ảnh trong project (nếu chưa có)
                string imageFolder = Path.Combine(Application.StartupPath, "img/portrait");
                
                if (!Directory.Exists(imageFolder))
                    Directory.CreateDirectory(imageFolder);

                // Tạo tên file đích (đảm bảo không trùng)
                string fileName = Path.GetFileName(selectedPath);
                string destPath = Path.Combine(imageFolder, fileName);

                // Nếu file trùng tên, đổi tên mới
                int count = 1;
                while (File.Exists(destPath))
                {
                    string nameOnly = Path.GetFileNameWithoutExtension(fileName);
                    string ext = Path.GetExtension(fileName);
                    destPath = Path.Combine(imageFolder, $"{nameOnly}_{count}{ext}");
                    count++;
                }

                // Sao chép ảnh vào thư mục project
                File.Copy(selectedPath, destPath);
                
                string finalFileName = Path.GetFileName(destPath);
                
                string relativePath = Path.Combine("img", "portrait", finalFileName).Replace("\\", "/");
                Console.WriteLine(relativePath);
                OnClickAddImg?.Invoke(relativePath);
            }
        }
    }
}