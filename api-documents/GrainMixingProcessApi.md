# Grain Mixing Process API

API để quản lý công đoạn pha hạt (Grain Mixing Process) trong hệ thống.

## Base URL
```
/api/grain-mixing-processes
```

## Authentication
Tất cả các endpoint đều yêu cầu xác thực thông qua JWT token trong header:
```
Authorization: Bearer {token}
```

---

## Endpoints

### 1. Lấy danh sách công đoạn pha hạt

**GET** `/api/grain-mixing-processes`

Lấy danh sách tất cả công đoạn pha hạt với phân trang và lọc.

#### Query Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| page | int | No | Số trang (mặc định: 1) |
| pageSize | int | No | Kích thước trang (mặc định: 10) |
| filter | string | No | Bộ lọc Gridify (ví dụ: `ProductionDate >= 2024-01-01`) |
| orderBy | string | No | Sắp xếp (ví dụ: `ProductionDate desc`) |

#### Response
```json
{
  "results": [
    {
      "id": 1,
      "isDraft": false,
      "productionDate": "2024-01-15T00:00:00",
      "workerCount": 5,
      "totalHoursWorked": 40.5,
      "laborProductivity": 123.45,
      "creatorId": 1,
      "modifierId": null,
      "createdAt": "2024-01-15T08:00:00",
      "modifiedAt": null,
      "creatorName": "Nguyễn Văn A",
      "modifierName": null,
      "lines": []
    }
  ],
  "totalCount": 100,
  "page": 1,
  "pageSize": 10
}
```

---

### 2. Lấy chi tiết công đoạn pha hạt

**GET** `/api/grain-mixing-processes/{id}`

Lấy thông tin chi tiết của một công đoạn pha hạt theo ID.

#### Path Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| id | int | Yes | ID của công đoạn pha hạt |

#### Response
```json
{
  "id": 1,
  "isDraft": false,
  "productionDate": "2024-01-15T00:00:00",
  "workerCount": 5,
  "totalHoursWorked": 40.5,
  "laborProductivity": 123.45,
  "creatorId": 1,
  "modifierId": null,
  "createdAt": "2024-01-15T08:00:00",
  "modifiedAt": null,
  "creatorName": "Nguyễn Văn A",
  "modifierName": null,
  "lines": [
    {
      "id": 1,
      "grainMixingProcessId": 1,
      "productionBatch": "LOT001",
      "cardCode": "C00001",
      "customerName": "Công ty ABC",
      "materialIssueVoucherNo": "MIT001",
      "mixtureType": "HD",
      "specification": "HD trắng sữa 30 micron",
      "workerId": 10,
      "workerName": "Trần Văn B",
      "machineName": "Máy pha 01",
      "startTime": "2024-01-15T08:00:00",
      "endTime": "2024-01-15T16:00:00",
      "ppTron": 0,
      "ppHdNhot": 0,
      "ppLdpe": 0,
      "ppDc": 0,
      "ppAdditive": 0,
      "ppColor": 0,
      "ppOther": 0,
      "ppRit": 0,
      "hdLldpe2320": 100.5,
      "hdRecycled": 20.0,
      "hdTalcol": 5.0,
      "hdDc": 2.0,
      "hdColor": 1.5,
      "hdOther": 0,
      "hdHd": 0,
      "peAdditive": 0,
      "peTalcol": 0,
      "peColor": 0,
      "peOther": 0,
      "peLdpe": 0,
      "peLldpe": 0,
      "peRecycled": 0,
      "peDc": 0,
      "shrinkRe707": 0,
      "shrinkSlip": 0,
      "shrinkStatic": 0,
      "shrinkDc": 0,
      "shrinkTalcol": 0,
      "shrinkOther": 0,
      "shrinkLldpe": 0,
      "shrinkRecycled": 0,
      "shrinkTangDai": 0,
      "wrapRecycledCa": 0,
      "wrapRecycledCb": 0,
      "wrapGlue": 0,
      "wrapColor": 0,
      "wrapDc": 0,
      "wrapLdpe": 0,
      "wrapLldpe": 0,
      "wrapSlip": 0,
      "wrapAdditive": 0,
      "wrapOther": 0,
      "wrapTangDaiC6": 0,
      "wrapTangDaiC8": 0,
      "evaPop3070": 0,
      "evaLdpe": 0,
      "evaDc": 0,
      "evaTalcol": 0,
      "evaSlip": 0,
      "evaStaticAdditive": 0,
      "evaOther": 0,
      "evaTgc": 0,
      "quantityKg": 129.0,
      "requiredDate": "2024-01-20T00:00:00",
      "isCompleted": true,
      "actualCompletionDate": "2024-01-15T16:00:00",
      "delayReason": null
    }
  ]
}
```

#### Error Responses
- **404 Not Found**: Không tìm thấy công đoạn pha hạt với ID đã cho

---

### 3. Tạo công đoạn pha hạt mới

**POST** `/api/grain-mixing-processes`

Tạo mới một công đoạn pha hạt.

#### Request Body
```json
{
  "isDraft": false,
  "productionDate": "2024-01-15T00:00:00",
  "workerCount": 5,
  "totalHoursWorked": 40.5,
  "lines": [
    {
      "productionBatch": "LOT001",
      "cardCode": "C00001",
      "materialIssueVoucherNo": "MIT001",
      "mixtureType": "HD",
      "specification": "HD trắng sữa 30 micron",
      "workerId": 10,
      "machineName": "Máy pha 01",
      "startTime": "2024-01-15T08:00:00",
      "endTime": "2024-01-15T16:00:00",
      "ppTron": 0,
      "ppHdNhot": 0,
      "ppLdpe": 0,
      "ppDc": 0,
      "ppAdditive": 0,
      "ppColor": 0,
      "ppOther": 0,
      "ppRit": 0,
      "hdLldpe2320": 100.5,
      "hdRecycled": 20.0,
      "hdTalcol": 5.0,
      "hdDc": 2.0,
      "hdColor": 1.5,
      "hdOther": 0,
      "hdHd": 0,
      "peAdditive": 0,
      "peTalcol": 0,
      "peColor": 0,
      "peOther": 0,
      "peLdpe": 0,
      "peLldpe": 0,
      "peRecycled": 0,
      "peDc": 0,
      "shrinkRe707": 0,
      "shrinkSlip": 0,
      "shrinkStatic": 0,
      "shrinkDc": 0,
      "shrinkTalcol": 0,
      "shrinkOther": 0,
      "shrinkLldpe": 0,
      "shrinkRecycled": 0,
      "shrinkTangDai": 0,
      "wrapRecycledCa": 0,
      "wrapRecycledCb": 0,
      "wrapGlue": 0,
      "wrapColor": 0,
      "wrapDc": 0,
      "wrapLdpe": 0,
      "wrapLldpe": 0,
      "wrapSlip": 0,
      "wrapAdditive": 0,
      "wrapOther": 0,
      "wrapTangDaiC6": 0,
      "wrapTangDaiC8": 0,
      "evaPop3070": 0,
      "evaLdpe": 0,
      "evaDc": 0,
      "evaTalcol": 0,
      "evaSlip": 0,
      "evaStaticAdditive": 0,
      "evaOther": 0,
      "evaTgc": 0,
      "quantityKg": 129.0,
      "requiredDate": "2024-01-20T00:00:00",
      "isCompleted": true,
      "actualCompletionDate": "2024-01-15T16:00:00",
      "delayReason": null
    }
  ]
}
```

#### Response
```json
{
  "id": 1,
  "isDraft": false,
  "productionDate": "2024-01-15T00:00:00",
  "workerCount": 5,
  "totalHoursWorked": 40.5,
  "laborProductivity": 3.185,
  "creatorId": 1,
  "modifierId": null,
  "createdAt": "2024-01-15T08:00:00",
  "modifiedAt": null,
  "creatorName": "Nguyễn Văn A",
  "modifierName": null,
  "lines": [...]
}
```

#### Error Responses
- **400 Bad Request**: Dữ liệu không hợp lệ
- **404 Not Found**: Không tìm thấy công nhân hoặc khách hàng
- **401 Unauthorized**: Không xác định được người dùng hiện tại

---

### 4. Cập nhật công đoạn pha hạt

**PUT** `/api/grain-mixing-processes/{id}`

Cập nhật thông tin của một công đoạn pha hạt.

#### Path Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| id | int | Yes | ID của công đoạn pha hạt cần cập nhật |

#### Request Body
```json
{
  "isDraft": false,
  "productionDate": "2024-01-15T00:00:00",
  "workerCount": 6,
  "totalHoursWorked": 48.0,
  "lines": [
    {
      "id": 1,
      "productionBatch": "LOT001-UPDATED",
      "cardCode": "C00001",
      "materialIssueVoucherNo": "MIT001",
      "mixtureType": "HD",
      "specification": "HD trắng sữa 30 micron - Updated",
      "workerId": 10,
      "machineName": "Máy pha 01",
      "startTime": "2024-01-15T08:00:00",
      "endTime": "2024-01-15T17:00:00",
      "ppTron": 0,
      "ppHdNhot": 0,
      "ppLdpe": 0,
      "ppDc": 0,
      "ppAdditive": 0,
      "ppColor": 0,
      "ppOther": 0,
      "ppRit": 0,
      "hdLldpe2320": 110.0,
      "hdRecycled": 22.0,
      "hdTalcol": 5.5,
      "hdDc": 2.2,
      "hdColor": 1.8,
      "hdOther": 0,
      "hdHd": 0,
      "peAdditive": 0,
      "peTalcol": 0,
      "peColor": 0,
      "peOther": 0,
      "peLdpe": 0,
      "peLldpe": 0,
      "peRecycled": 0,
      "peDc": 0,
      "shrinkRe707": 0,
      "shrinkSlip": 0,
      "shrinkStatic": 0,
      "shrinkDc": 0,
      "shrinkTalcol": 0,
      "shrinkOther": 0,
      "shrinkLldpe": 0,
      "shrinkRecycled": 0,
      "shrinkTangDai": 0,
      "wrapRecycledCa": 0,
      "wrapRecycledCb": 0,
      "wrapGlue": 0,
      "wrapColor": 0,
      "wrapDc": 0,
      "wrapLdpe": 0,
      "wrapLldpe": 0,
      "wrapSlip": 0,
      "wrapAdditive": 0,
      "wrapOther": 0,
      "wrapTangDaiC6": 0,
      "wrapTangDaiC8": 0,
      "evaPop3070": 0,
      "evaLdpe": 0,
      "evaDc": 0,
      "evaTalcol": 0,
      "evaSlip": 0,
      "evaStaticAdditive": 0,
      "evaOther": 0,
      "evaTgc": 0,
      "quantityKg": 141.5,
      "requiredDate": "2024-01-20T00:00:00",
      "isCompleted": true,
      "actualCompletionDate": "2024-01-15T17:00:00",
      "delayReason": null
    }
  ]
}
```

#### Response
```json
{
  "id": 1,
  "isDraft": false,
  "productionDate": "2024-01-15T00:00:00",
  "workerCount": 6,
  "totalHoursWorked": 48.0,
  "laborProductivity": 2.948,
  "creatorId": 1,
  "modifierId": 1,
  "createdAt": "2024-01-15T08:00:00",
  "modifiedAt": "2024-01-16T10:30:00",
  "creatorName": "Nguyễn Văn A",
  "modifierName": "Nguyễn Văn A",
  "lines": [...]
}
```

#### Error Responses
- **400 Bad Request**: Dữ liệu không hợp lệ
- **404 Not Found**: Không tìm thấy công đoạn pha hạt, công nhân hoặc khách hàng
- **401 Unauthorized**: Không xác định được người dùng hiện tại

---

### 5. Xóa công đoạn pha hạt

**DELETE** `/api/grain-mixing-processes/{id}`

Xóa một công đoạn pha hạt.

#### Path Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| id | int | Yes | ID của công đoạn pha hạt cần xóa |

#### Response
- **204 No Content**: Xóa thành công

#### Error Responses
- **404 Not Found**: Không tìm thấy công đoạn pha hạt với ID đã cho

---

## Data Models

### GrainMixingProcess
| Field | Type | Description |
|-------|------|-------------|
| id | int | ID của công đoạn pha hạt |
| isDraft | bool | Bản nháp |
| productionDate | datetime | Ngày sản xuất (ngày pha) |
| workerCount | int | Tổng số nhân công |
| totalHoursWorked | double | Tổng số giờ làm việc |
| laborProductivity | double | Năng suất lao động (Kg/giờ) - Tự động tính |
| creatorId | short | ID người tạo |
| modifierId | short? | ID người sửa cuối |
| createdAt | datetime | Thời gian tạo |
| modifiedAt | datetime? | Thời gian sửa cuối |
| creatorName | string? | Tên người tạo |
| modifierName | string? | Tên người sửa cuối |
| lines | array | Danh sách chi tiết công đoạn pha hạt |

### GrainMixingProcessLine
| Field | Type | Description |
|-------|------|-------------|
| id | int | ID của chi tiết |
| grainMixingProcessId | int | ID công đoạn pha hạt |
| productionBatch | string? | Lô sản xuất |
| cardCode | string? | Mã khách hàng |
| customerName | string? | Tên khách hàng (từ BusinessPartner) |
| materialIssueVoucherNo | string? | Số phiếu lĩnh vật tư |
| mixtureType | string? | Chủng loại pha (HD, PE, Màng co, Màng chít, PP, EVA) |
| specification | string? | Quy cách diễn giải |
| workerId | int? | ID công nhân pha |
| workerName | string? | Tên công nhân pha |
| machineName | string? | Tên máy pha |
| startTime | datetime? | Thời gian bắt đầu pha |
| endTime | datetime? | Thời gian kết thúc pha |
| ppTron | decimal | Hạt PP trơn (PP) |
| ppHdNhot | decimal | Hạt HD nhớt (PP) |
| ppLdpe | decimal | Hạt LDPE (PP) |
| ppDc | decimal | Hạt DC (PP) |
| ppAdditive | decimal | Phụ gia (PP) |
| ppColor | decimal | Hạt màu (PP) |
| ppOther | decimal | Hạt khác (PP) |
| ppRit | decimal | Hạt Rit (PP) |
| hdLldpe2320 | decimal | Hạt LLDPE 2320 (HD) |
| hdRecycled | decimal | Hạt tái chế (HD) |
| hdTalcol | decimal | Hạt Talcol (HD) |
| hdDc | decimal | Hạt DC (HD) |
| hdColor | decimal | Hạt màu (HD) |
| hdOther | decimal | Hạt khác (HD) |
| hdHd | decimal | Hạt HD (HD) |
| peAdditive | decimal | Phụ gia (PE) |
| peTalcol | decimal | Talcol (PE) |
| peColor | decimal | Hạt màu (PE) |
| peOther | decimal | Hạt khác (PE) |
| peLdpe | decimal | Hạt LDPE (PE) |
| peLldpe | decimal | Hạt LLDPE (PE) |
| peRecycled | decimal | Hạt tái chế (PE) |
| peDc | decimal | Hạt DC (PE) |
| shrinkRe707 | decimal | Tăng R8707 (Màng co) |
| shrinkSlip | decimal | Tăng Slip (Màng co) |
| shrinkStatic | decimal | Tăng tĩnh điện (Màng co) |
| shrinkDc | decimal | Hạt DC (Màng co) |
| shrinkTalcol | decimal | Talcol (Màng co) |
| shrinkOther | decimal | Hạt khác (Màng co) |
| shrinkLldpe | decimal | Hạt LLDPE (Màng co) |
| shrinkRecycled | decimal | Hạt tái chế (Màng co) |
| shrinkTangDai | decimal | Tăng dài (Màng co) |
| wrapRecycledCa | decimal | Hạt tái chế Ca (Màng chít) |
| wrapRecycledCb | decimal | Hạt tái chế Cb (Màng chít) |
| wrapGlue | decimal | Keo (Màng chít) |
| wrapColor | decimal | Hạt màu (Màng chít) |
| wrapDc | decimal | Hạt DC (Màng chít) |
| wrapLdpe | decimal | Hạt LDPE (Màng chít) |
| wrapLldpe | decimal | Hạt LLDPE (Màng chít) |
| wrapSlip | decimal | Hạt Slip (Màng chít) |
| wrapAdditive | decimal | Phụ gia (Màng chít) |
| wrapOther | decimal | Hạt khác (Màng chít) |
| wrapTangDaiC6 | decimal | Tăng dài C6 (Màng chít) |
| wrapTangDaiC8 | decimal | Tăng dài C8 (Màng chít) |
| evaPop3070 | decimal | POP 3070 (EVA) |
| evaLdpe | decimal | Hạt LDPE (EVA) |
| evaDc | decimal | Hạt DC (EVA) |
| evaTalcol | decimal | Hạt Talcol (EVA) |
| evaSlip | decimal | Slip (EVA) |
| evaStaticAdditive | decimal | Trợ tĩnh chống dính (EVA) |
| evaOther | decimal | Hạt khác (EVA) |
| evaTgc | decimal | TGC (EVA) |
| quantityKg | decimal | Sản lượng pha (Kg) |
| requiredDate | datetime? | Ngày cần hàng |
| isCompleted | bool | Xác nhận hoàn thành |
| actualCompletionDate | datetime? | Ngày hoàn thành thực tế |
| delayReason | string? | Nguyên nhân chậm tiến độ |

---

## Business Logic

### Tính năng suất lao động
Năng suất lao động được tự động tính theo công thức:
```
LaborProductivity = TotalQuantityKg / TotalHoursWorked
```
Trong đó:
- `TotalQuantityKg` = Tổng sản lượng pha của tất cả các lines (sum của `quantityKg`)
- `TotalHoursWorked` = Tổng số giờ làm việc

### Quản lý Lines
- Khi cập nhật, các line có `id` sẽ được cập nhật
- Các line không có `id` sẽ được tạo mới
- Các line không xuất hiện trong request body sẽ bị xóa

### Validation
- `productionDate` là bắt buộc
- `workerCount` phải >= 0
- `totalHoursWorked` phải >= 0
- `quantityKg` phải >= 0
- `workerId` phải tồn tại trong bảng Employees (nếu được cung cấp)
- `cardCode` phải tồn tại trong bảng BusinessPartners (nếu được cung cấp)

---

## Examples

### Ví dụ 1: Lấy danh sách với filter và sắp xếp
```bash
GET /api/grain-mixing-processes?page=1&pageSize=20&filter=ProductionDate >= 2024-01-01 AND IsDraft = false&orderBy=ProductionDate desc
```

### Ví dụ 2: Tạo công đoạn pha HD
```bash
POST /api/grain-mixing-processes
Content-Type: application/json
Authorization: Bearer {token}

{
  "isDraft": false,
  "productionDate": "2024-01-15T00:00:00",
  "workerCount": 3,
  "totalHoursWorked": 24.0,
  "lines": [
    {
      "productionBatch": "HD-LOT001",
      "cardCode": "C00001",
      "materialIssueVoucherNo": "MIT-HD-001",
      "mixtureType": "HD",
      "specification": "HD trắng sữa 30 micron",
      "workerId": 10,
      "machineName": "Máy pha HD-01",
      "startTime": "2024-01-15T08:00:00",
      "endTime": "2024-01-15T16:00:00",
      "hdLldpe2320": 100.0,
      "hdRecycled": 20.0,
      "hdTalcol": 5.0,
      "hdDc": 2.0,
      "hdColor": 1.5,
      "quantityKg": 128.5,
      "requiredDate": "2024-01-20T00:00:00",
      "isCompleted": true,
      "actualCompletionDate": "2024-01-15T16:00:00"
    }
  ]
}
```

### Ví dụ 3: Cập nhật công đoạn pha PE
```bash
PUT /api/grain-mixing-processes/1
Content-Type: application/json
Authorization: Bearer {token}

{
  "isDraft": false,
  "productionDate": "2024-01-15T00:00:00",
  "workerCount": 4,
  "totalHoursWorked": 32.0,
  "lines": [
    {
      "id": 1,
      "productionBatch": "PE-LOT001-UPDATED",
      "cardCode": "C00002",
      "materialIssueVoucherNo": "MIT-PE-001",
      "mixtureType": "PE",
      "specification": "PE trong suốt 40 micron",
      "workerId": 11,
      "machineName": "Máy pha PE-01",
      "startTime": "2024-01-15T08:00:00",
      "endTime": "2024-01-15T16:00:00",
      "peLdpe": 80.0,
      "peLldpe": 60.0,
      "peRecycled": 15.0,
      "peAdditive": 3.0,
      "peTalcol": 2.0,
      "peColor": 1.0,
      "quantityKg": 161.0,
      "requiredDate": "2024-01-22T00:00:00",
      "isCompleted": true,
      "actualCompletionDate": "2024-01-15T16:00:00"
    }
  ]
}
```
