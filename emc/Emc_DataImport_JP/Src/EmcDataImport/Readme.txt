Hướng dẫn chạy service EmcDataImport
Step 1: Chạy project bằng Visual Studio
Step 2: Build Solution 'EmcDataImport' ở chế độ Release
Step 3: Sao khi build vào thư mục project tim thư mục bin/Realease
Step 4: Chạy file INSTALL_x64.bat (nếu windown 64bit) hoặc INSTALL_x86.bat( Windown 32bit) ở chế độ Administrator.
Step 5: Vào kiểm tra service trong windown tìm tên service là: EmcDataImport 
Step 6: Vào cấu hình file config để chạy service: 'EmcDataImport.exe.config'
Step 7: Click chuột phải vào service chọn start service.

*Chú ý: Nếu chạy file INSTALL xong mà không kiểm tra trong dịch vụ của windown vẫn chưa thấy thì mở file INSTALL_x64.bat( hoặc INSTALL_x86.bat) thêm đường dẫn chính xác đến file EmcDataImport.exe
