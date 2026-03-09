let gioHang = [];
function locSanPham(loaiCanTim) {
    let cacSanPham = document.querySelectorAll('.the-san-pham');

    cacSanPham.forEach(function (sanpham) {
        let loaiSanPham = sanpham.getAttribute('data-loai');

        if (loaiCanTim === 'tatca') {
            sanpham.style.display = 'block';
        } else {
            if (loaiSanPham === loaiCanTim) {
                sanpham.style.display = 'block';
            } else {
                sanpham.style.display = 'none';
            }
        }
    });
}
//Giỏ Hàng
function moGioHang() {
    document.getElementById('man-che-mo').style.display = 'block';
    document.getElementById('popup-gio-hang').classList.add('hien-len');
    renderGioHang();
}

function dongGioHang() {
    document.getElementById('man-che-mo').style.display = 'none';
    document.getElementById('popup-gio-hang').classList.remove('hien-len');
}

function hienFormDangNhap() {
    document.getElementById('man-che-mo').style.display = 'block';
    document.getElementById('khung-dang-nhap-dang-ky').classList.add('hien-len');
}

function dongForm() {
    document.getElementById('man-che-mo').style.display = 'none';
    document.getElementById('khung-dang-nhap-dang-ky').classList.remove('hien-len');
    document.getElementById('form-doi-matkhau').classList.remove('hien-len');
}

// --- 2. Chuyển đổi qua lại giữa Tab Đăng Nhập / Đăng Ký ---
function chuyenTab(loaiTab) {
    // Ẩn hết cả 2 form
    document.getElementById('form-dang-nhap').style.display = 'none';
    document.getElementById('form-dang-ky').style.display = 'none';

    // Reset style nút tab
    let cacNut = document.querySelectorAll('.tab-btn');
    cacNut.forEach(nut => nut.classList.remove('dang-chon'));

    if (loaiTab === 'dang-nhap') {
        document.getElementById('form-dang-nhap').style.display = 'block';
        cacNut[0].classList.add('dang-chon');
    } else {
        document.getElementById('form-dang-ky').style.display = 'block';
        cacNut[1].classList.add('dang-chon');
    }
}

let daDangNhap = false;

async function xuLyDangNhap() {
    // Lấy dữ liệu người dùng nhập
    let sdt = document.getElementById('dn-sdt').value;
    let pass = document.getElementById('dn-matkhau').value;

    let duLieu = {
        SoDienThoai: sdt,
        MatKhau: pass
    };
    try {
        let response = await fetch('/api/simpleauth/dangnhap', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(duLieu)
        });

        let result = await response.json();

        if (response.ok) {
            hienThongBao(result.message)
            if (result.chucVu == "employee") {
                window.location.href = "/NhanVien";
            }
            else
            {
                document.getElementById('nut-dang-nhap').style.display = 'none';
                document.getElementById('khung-nguoi-dung').style.display = 'flex';
                document.getElementById('ten-khach-hang').innerText = result.hoTen;
                daDangNhap = true;
                dongForm();
            }
        } else {
            hienThongBao(result.message);
        }
    } catch (loi) {
        console.log(loi);
        hienThongBao("Không kết nối được với Server!");
    }
}

async function xuLyDangKy() {
    let hoten = document.getElementById('dk-hoten').value;
    let sdt = document.getElementById('dk-sdt').value;
    let pass = document.getElementById('dk-matkhau').value;

    let duLieu = {
        HoTen: hoten,
        SoDienThoai: sdt,
        MatKhau: pass
    };

    try {
        let response = await fetch('/api/simpleauth/dangky', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(duLieu)
        });

        let result = await response.json();

        if (response.ok) {
            hienThongBao(result.message)
            chuyenTab('dang-nhap');
        } else {
            hienThongBao(result.message);
        }
    } catch (loi) {
        console.log(loi);
        hienThongBao("Không kết nối được với Server!");
    }
}

// --- 4. Logic cho nút "Thêm" (Mua hàng) ---

document.addEventListener('keydown', function (event) {
    if (event.key === "Escape") {
        dongForm();
    }
});
// Phần hiển thị thông báo
function hienThongBao(noiDung) {
    var x = document.getElementById("thong-bao-custom");
    var text = document.getElementById("noi-dung-thong-bao");

    text.innerText = noiDung;
    x.className = "thong-bao hien-len";

    // set thời gian trước khi tắt
    setTimeout(function () {
        x.className = x.className.replace("hien-len", "");
    }, 1500);
}
const khungUser = document.getElementById("khung-nguoi-dung");
const menuUser = document.getElementById("menu-user");

khungUser.addEventListener("click", function (event) {
    event.stopPropagation();
    menuUser.style.display =
        menuUser.style.display === "flex" ? "none" : "flex";
});

document.addEventListener("click", function () {
    menuUser.style.display = "none";
});

function doiMatKhau() {
    menuUser.style.display = "none";
    document.getElementById('man-che-mo').style.display = 'block';
    document.getElementById('form-doi-matkhau').classList.add('hien-len');
}

function dongFormDoiMK() {
    document.getElementById('man-che-mo').style.display = 'none';
    document.getElementById('form-doi-matkhau').classList.remove('hien-len');
}

function dangXuat() {
    hienThongBao("Đăng xuất thành công!");
    localStorage.removeItem("user");
    window.location.reload();
}
async function xuLyDoiPass(){
    let sdt = document.getElementById("mk-sdt").value;
    let mkcu = document.getElementById("mk-cu").value;
    let mkmoi = document.getElementById("mk-moi").value;
    let xacnhanmkmoi = document.getElementById("xacnhan-mk-moi").value;
    let duLieu = {
        SoDienThoai: sdt, MatKhauCu: mkcu, MatKhauMoi: mkmoi, XacNhanMatKhauMoi: xacnhanmkmoi
    };
    try {
        let response = await fetch('/api/simpleauth/doimatkhau', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(duLieu)
        });

        let result = await response.json();

        if (response.ok) {
            hienThongBao(result.message)
            dongForm();
        } else {
            hienThongBao(result.message);
        }
    } catch (loi) {
        hienThongBao("Không kết nối được với server");
    }
}
//gửi lên database
async function xacNhanDon() {
    let gioNhan = document.getElementById("gio-nhan").value;
    if (gioHang.length === 0) {
        hienThongBao("Giỏ hàng đang trống!");
        return;
    }
    if (!gioNhan) {
        hienThongBao("Vui lòng chọn thời gian nhận hàng!");
        return;
    }
    const tenkhach = document.getElementById('ten-khach-hang').innerText;
    const gionhan = document.getElementById('gio-nhan').value;
    const data = {
        TenKhach: tenkhach,
        GioNhan: gionhan,
        ChiTietDonHangs: gioHang.map(item => ({
            hangHoaId: item.id,
            soLuong: item.soLuong,
        }))
    };
    try {
        const response = await fetch('/api/DonHang/donhang', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        });

        const result = await response.json();

        if (response.ok) {
            hienThongBao(result.message);
            gioHang = [];
            renderGioHang();
            dongGioHang();
        } else {
            hienThongBao("Lỗi đặt hàng!");
        }

    } catch (error) {
        console.error(error);
        hienThongBao("Lỗi kết nối server!");
    }
}
function renderGioHang() {
    console.log(gioHang);

    let container = document.getElementById("danh-sach-gio");
    container.innerHTML = "";

    if (gioHang.length === 0) {
        container.innerHTML = "<p>Giỏ hàng trống</p>";
        return;
    }

    let tong = 0;

    gioHang.forEach(item => {

        let thanhTien = item.gia * item.soLuong;
        tong += thanhTien;

        container.innerHTML += `
            <div style="margin-bottom:10px;">
                <strong>${item.ten}</strong><br>
                ${item.soLuong} × ${item.gia.toLocaleString()}đ
                = ${thanhTien.toLocaleString()}đ
            </div>
        `;
    });

    container.innerHTML += `
        <hr>
        <strong>Tổng tiền: ${tong.toLocaleString()}đ</strong>
    `;
}
function themvaogio(button,id) {
    if (!daDangNhap) {
        hienThongBao("Vui Lòng Đăng Nhập!");
        return;
    }
    let theSanPham = button.closest(".the-san-pham");

    let ten = theSanPham.querySelector("h3").innerText;
    let giaText = theSanPham.querySelector(".gia-tien").innerText;
    let gia = parseInt(giaText.replace(/\D/g, ""));
    let mon = gioHang.find(item => item.ten === ten);

    if (mon) {
        mon.soLuong += 1;
    } else {
        gioHang.push({
            id : id,
            ten: ten,
            gia: gia,
            soLuong: 1
        });
    }
    renderGioHang();
    hienThongBao("Đã thêm vào giỏ!");
}
