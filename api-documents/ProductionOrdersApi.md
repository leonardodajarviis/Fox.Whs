# Production Orders API Documentation

API quản lý danh sách Production Orders (Lệnh sản xuất) từ SAP.

## Base URL
```
/api/production-orders
```

## Authentication
Tất cả các endpoint yêu cầu xác thực bằng Bearer Token trong header:
```
Authorization: Bearer {access_token}
```

---

## Endpoints

### 1. Lấy danh sách Production Orders

Lấy danh sách các lệnh sản xuất với phân trang và bộ lọc.

**Endpoint:**
```
GET /api/production-orders
```

**Query Parameters:**

| Tham số | Kiểu dữ liệu | Bắt buộc | Mô tả | Giá trị mặc định |
|---------|--------------|----------|-------|------------------|
| `page` | integer | Không | Số trang cần lấy (tối thiểu: 1) | 1 |
| `pageSize` | integer | Không | Số bản ghi trên mỗi trang (1-100) | 10 |
| `type` | string | Không | Loại công đoạn sản xuất | - |
| `itemCode` | string | Không | Mã hàng hóa để lọc | - |

**Giá trị hợp lệ cho tham số `type`:**
- `printing` - Công đoạn in
- `blowing` - Công đoạn thổi
- `rewinding` - Công đoạn tua
- `cutting` - Công đoạn cắt
- `slitting` - Công đoạn chia

**Response:**

Status Code: `200 OK`

```json
{
  "page": 1,
  "pageSize": 10,
  "totalCount": 100,
  "results": [
    {
      "docEntry": 12345,
      "itemCode": "ITEM001",
      "docNum": "PO-001",
      "postDate": "2025-11-07T00:00:00",
      "dueDate": "2025-11-15T00:00:00",
      "plannedQty": 1000,
      "completedQty": 500,
      "status": "R",
      "isPrinting": "Y",
      "printingStatus": "N",
      "isBlowing": "N",
      "blowingStatus": null,
      "isRewinding": "N",
      "rewindingStatus": null,
      "isCutting": "N",
      "cuttingStatus": null,
      "isSlitting": "N",
      "slittingStatus": null,
      "itemDetail": {
        "itemCode": "ITEM001",
        "itemName": "Tên sản phẩm",
        "productType": "PT01",
        "productTypeInfo": {
          "code": "PT01",
          "name": "Loại sản phẩm"
        }
      },
      "businessPartnerDetail": {
        "cardCode": "BP001",
        "cardName": "Tên đối tác"
      }
    }
  ]
}
```

---

## Ví dụ sử dụng

### 1. Lấy tất cả Production Orders (trang đầu tiên)

**Request:**
```bash
curl -X GET "http://localhost:5000/api/production-orders?page=1&pageSize=10" \
  -H "Authorization: Bearer {your_token}"
```

**Response:**
```json
{
  "page": 1,
  "pageSize": 10,
  "totalCount": 50,
  "results": [...]
}
```

### 2. Lấy Production Orders chưa hoàn thành công đoạn in

**Request:**
```bash
curl -X GET "http://localhost:5000/api/production-orders?type=printing&page=1&pageSize=20" \
  -H "Authorization: Bearer {your_token}"
```

**Response:**
```json
{
  "page": 1,
  "pageSize": 20,
  "totalCount": 15,
  "results": [...]
}
```

### 3. Lọc theo mã hàng hóa cụ thể

**Request:**
```bash
curl -X GET "http://localhost:5000/api/production-orders?itemCode=ITEM001&page=1&pageSize=10" \
  -H "Authorization: Bearer {your_token}"
```

**Response:**
```json
{
  "page": 1,
  "pageSize": 10,
  "totalCount": 5,
  "results": [...]
}
```

### 4. Kết hợp nhiều bộ lọc

**Request:**
```bash
curl -X GET "http://localhost:5000/api/production-orders?type=blowing&itemCode=ITEM002&page=1&pageSize=15" \
  -H "Authorization: Bearer {your_token}"
```

**Response:**
```json
{
  "page": 1,
  "pageSize": 15,
  "totalCount": 3,
  "results": [...]
}
```

---

## Mã lỗi

### 400 Bad Request
Yêu cầu không hợp lệ.

**Ví dụ:**
```json
{
  "type": "BadRequest",
  "message": "Page phải lớn hơn 0",
  "statusCode": 400
}
```

**Nguyên nhân:**
- Tham số `page` nhỏ hơn 1
- Tham số `pageSize` không nằm trong khoảng 1-100

### 401 Unauthorized
Token xác thực không hợp lệ hoặc không được cung cấp.

```json
{
  "type": "Unauthorized",
  "message": "Unauthorized",
  "statusCode": 401
}
```

### 500 Internal Server Error
Lỗi server nội bộ.

```json
{
  "type": "InternalServerError",
  "message": "Đã xảy ra lỗi không mong muốn",
  "statusCode": 500
}
```

---

## Lưu ý

1. **Phân trang:** 
   - Giá trị `page` phải lớn hơn 0
   - Giá trị `pageSize` phải từ 1 đến 100

2. **Bộ lọc theo loại công đoạn:**
   - Chỉ lấy các Production Orders có trạng thái chưa hoàn thành ("N" hoặc null) cho công đoạn tương ứng
   - Và có cờ công đoạn được bật ("Y")

3. **Dữ liệu bao gồm:**
   - Thông tin chi tiết hàng hóa (`ItemDetail`)
   - Thông tin loại sản phẩm (`ProductTypeInfo`)
   - Thông tin đối tác kinh doanh (`BusinessPartnerDetail`)

4. **Sắp xếp:**
   - Kết quả được sắp xếp theo `DocEntry` tăng dần

---

## Changelog

- **v1.0** (2025-11-07): Phiên bản đầu tiên
