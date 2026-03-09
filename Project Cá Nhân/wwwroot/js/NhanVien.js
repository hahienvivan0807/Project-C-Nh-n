    async function loadDonHang() {
    try {
        let response = await fetch('/api/DonHang/danhsach');
    let data = await response.json();

    let container = document.getElementById("donhang-list");
    container.innerHTML = "";

        data.forEach(don => {

        let chiTietHTML = "";
            don.chiTiet.forEach(ct => {
        chiTietHTML += `
                    <span class="don-tag">
                        ${ct.tenHang} × ${ct.soLuong}
                    </span>
                `;
            });

    container.innerHTML += `
    <div class="donhang-item">
        <div class="container-don">
            <div class="don-number">
                <h2>Đơn #${don.id}</h2>
                <span class="don-time">${don.gioNhan}</span>
            </div>
            <div class="don-details">
                ${chiTietHTML}
            </div>
            <div class="don-price">
                ${don.tongTien.toLocaleString()}đ
            </div>
            <div class="don-status pending">
                ${don.trangThai}
            </div>
        </div>
    </div>
    `;
        });

    document.querySelector(".badge-count").innerText = data.length;

    } catch (error) {
        console.log(error);
    }
}

    loadDonHang();

    setInterval(loadDonHang, 5000);
