# SWTesting_Proj3_Selenium_Test

Đây là project cho bài tập lớn 3 - HK232 môn Kiểm tra phần mềm.

## Hướng dẫn chạy project trên Visual Studio cho các thành viên muốn dùng C# (hoặc đơn giản muốn chạy thử project để tham khảo)

Trước hết, do repo này được tạo bởi Visual Studio bản 2019, nên **thư mục gốc sẽ là `master` thay vì `main`**.

Để chạy project trên local, trước tiên clone project với url bằng terminal/powershell như bình thường (đừng sử dụng chức năng clone project của Visual Studio).

Sau đó, mở file `SWTesting_Proj3_Selenium_Test_C_Sharp.sln` (file quản lý solution của Visual Studio). Visual Studio sẽ tự mở.

Tiếp theo là cài thư viện (lưu ý: tương đối nặng, khuyên nên có sẵn ít nhất 500MB trống):
- Trong ứng dụng Visual Studio, nhấp vào thư mục `Selenium_Test_C_Sharp` để xổ thư mục xuống.
- Nhấn phải chuột vào `References` -> `Manage NuGet Packages...`
- Khi này, trên đầu tab vửa mở ra sẽ có thông báo dạng "Some NuGet packages are missing...". Nhấn `Restore` để cài đặt tự động.
- Sau khi cài đặt xong, **phải tắt Visual Studio** rồi mở lại file `SWTesting_Proj3_Selenium_Test_C_Sharp.sln`. Khi này, các thư viện mới được nạp đủ và mới chạy được.
- Để chạy thử, cứ nhấn F5. Project sẽ tự biên dịch và khởi chạy.

Hiện tại, mới chỉ có chức năng Probability Calculator (phần của Nghĩa) được hiện thực. Nên các phần sau đây chỉ đúng với project hiện tại. **Vui lòng PHẢI cập nhật phần này nếu có thay đổi trong tương lai**:
- Khi project khởi chạy xong sẽ xuất hiện một cửa sổ terminal, và một cái cửa sổ UI đơn giản với dòng chữ: "Start Probability Calculator Test".
- Cứ nhấn vào đó (tuỳ cấu hình máy mà tốc độ nhanh chậm) nó sẽ bắt đầu tự động mở Chrome và chạy tự động các testcase của chức năng Probability Calculator.
- Sau khi xong hết, của sổ Chrome sẽ tự tắt. Sau đó các bạn tắt hết luôn các cửa sổ xuất hiện do khởi chạy code.
- Khi này, trong phần Output của Visual Studio, `Show output from: Debug` sẽ có các dòng `Testcase ...: PASS` (nên là PASS :>, nó mà FAIL thì chịu).

## Một số lưu ý về project (phải đọc):

- Phần data input cho các testcase của chức năng Probability Calculator được đặt trong file `Selenium_Test/bin/Debug/Testcase_Probability.xlsx`. File này đã được cấu hình trong `.gitignore` để không bị bỏ qua, do đó khi clone project về chắc chắn phải có file này.
- Nếu định code tiếp C# trên nền project hiện tại của mình thì cũng **phải cấu hình `.gitignore` để file data input của các bạn không bị bỏ qua** (tham khảo cách thiết lập của mình nếu cần).
- Và nhớ, **tạo file data input khác cho các chức năng khác nhau, không viết vào cùng file**. Vì mỗi chức năng có format input và output khác nhau, nên đừng dùng chung file, nó chắc chắn sẽ bị loạn, và cực kỳ khó xử lý đọc file.
- Nếu các bạn không viết bằng C#, khuyên là tạo một thư mục mới ngoài thư mục gốc (master), đừng thêm trong thư mục `Selenium_Test` để không bị lẫn với những file của C# project.

## Công nghệ sử dụng:

- Selenium WebDriver
- Selenium.Chrome.WebDriver
- C# - WPF App (.NET Framework)
- ...
