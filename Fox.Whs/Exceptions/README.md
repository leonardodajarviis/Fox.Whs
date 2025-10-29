# Custom Exceptions Guide

## Tổng quan
Các custom exception được tạo để xử lý lỗi một cách nhất quán trong toàn bộ ứng dụng. Global Exception Handler sẽ tự động chuyển đổi các exception này thành error response với HTTP status code phù hợp.

## Danh sách Custom Exceptions

### 1. BadRequestException (400 Bad Request)
Sử dụng khi request không hợp lệ hoặc thiếu dữ liệu bắt buộc.

```csharp
// Ví dụ 1: Message đơn giản
throw new BadRequestException("Tên đăng nhập không được để trống");

// Ví dụ 2: Với inner exception
throw new BadRequestException("Dữ liệu không hợp lệ", innerException);
```

**Response:**
```json
{
  "statusCode": 400,
  "message": "Tên đăng nhập không được để trống",
  "details": null,
  "timestamp": "2025-10-29T10:30:00Z"
}
```

---

### 2. NotFoundException (404 Not Found)
Sử dụng khi không tìm thấy tài nguyên được yêu cầu.

```csharp
// Ví dụ 1: Message đơn giản
throw new NotFoundException("Người dùng không tồn tại");

// Ví dụ 2: Với tên resource và key
throw new NotFoundException("User", userId);
// => "User với id '123' không tồn tại"

// Ví dụ 3: Với inner exception
throw new NotFoundException("Sản phẩm không tìm thấy", innerException);
```

**Response:**
```json
{
  "statusCode": 404,
  "message": "User với id '123' không tồn tại",
  "details": null,
  "timestamp": "2025-10-29T10:30:00Z"
}
```

---

### 3. UnauthorizedException (401 Unauthorized)
Sử dụng khi người dùng chưa xác thực hoặc token không hợp lệ.

```csharp
// Ví dụ 1: Message mặc định
throw new UnauthorizedException();
// => "Bạn không có quyền truy cập"

// Ví dụ 2: Message tùy chỉnh
throw new UnauthorizedException("Token không hợp lệ hoặc đã hết hạn");

// Ví dụ 3: Với inner exception
throw new UnauthorizedException("Xác thực thất bại", innerException);
```

**Response:**
```json
{
  "statusCode": 401,
  "message": "Token không hợp lệ hoặc đã hết hạn",
  "details": null,
  "timestamp": "2025-10-29T10:30:00Z"
}
```

---

### 4. ForbiddenException (403 Forbidden)
Sử dụng khi người dùng đã xác thực nhưng không có quyền thực hiện hành động.

```csharp
// Ví dụ 1: Message mặc định
throw new ForbiddenException();
// => "Bạn không có quyền thực hiện hành động này"

// Ví dụ 2: Message tùy chỉnh
throw new ForbiddenException("Bạn không có quyền xóa user này");

// Ví dụ 3: Với inner exception
throw new ForbiddenException("Truy cập bị từ chối", innerException);
```

**Response:**
```json
{
  "statusCode": 403,
  "message": "Bạn không có quyền xóa user này",
  "details": null,
  "timestamp": "2025-10-29T10:30:00Z"
}
```

---

### 5. ConflictException (409 Conflict)
Sử dụng khi có xung đột dữ liệu (ví dụ: duplicate key, race condition).

```csharp
// Ví dụ 1: Message đơn giản
throw new ConflictException("Tên đăng nhập đã tồn tại");

// Ví dụ 2: Với inner exception
throw new ConflictException("Email đã được sử dụng", innerException);
```

**Response:**
```json
{
  "statusCode": 409,
  "message": "Tên đăng nhập đã tồn tại",
  "details": null,
  "timestamp": "2025-10-29T10:30:00Z"
}
```

---

### 6. ValidationException (422 Unprocessable Entity)
Sử dụng khi có lỗi validation với nhiều field.

```csharp
// Ví dụ 1: Message đơn giản
throw new ValidationException("Dữ liệu validation thất bại");

// Ví dụ 2: Với dictionary errors
var errors = new Dictionary<string, string[]>
{
    { "username", new[] { "Username phải từ 3-20 ký tự", "Username không được chứa ký tự đặc biệt" } },
    { "email", new[] { "Email không hợp lệ" } },
    { "password", new[] { "Password phải ít nhất 8 ký tự" } }
};
throw new ValidationException(errors);

// Ví dụ 3: Với message và errors
throw new ValidationException("Thông tin không hợp lệ", errors);
```

**Response:**
```json
{
  "statusCode": 422,
  "message": "Một hoặc nhiều lỗi validation xảy ra",
  "details": "Vui lòng kiểm tra lại dữ liệu đầu vào",
  "errors": {
    "username": [
      "Username phải từ 3-20 ký tự",
      "Username không được chứa ký tự đặc biệt"
    ],
    "email": ["Email không hợp lệ"],
    "password": ["Password phải ít nhất 8 ký tự"]
  },
  "timestamp": "2025-10-29T10:30:00Z"
}
```

---

## Ví dụ sử dụng trong Service

```csharp
public class ProductService
{
    public async Task<Product> GetProductAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        
        if (product == null)
        {
            throw new NotFoundException("Product", id);
        }
        
        return product;
    }

    public async Task<Product> CreateProductAsync(CreateProductDto dto)
    {
        // Validate
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new BadRequestException("Tên sản phẩm không được để trống");
        }

        // Check duplicate
        var exists = await _context.Products
            .AnyAsync(p => p.Code == dto.Code);
        
        if (exists)
        {
            throw new ConflictException($"Sản phẩm với mã '{dto.Code}' đã tồn tại");
        }

        // Create product
        var product = new Product
        {
            Name = dto.Name,
            Code = dto.Code
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return product;
    }

    public async Task DeleteProductAsync(int id, User currentUser)
    {
        var product = await _context.Products.FindAsync(id);
        
        if (product == null)
        {
            throw new NotFoundException("Product", id);
        }

        // Check permission
        if (!currentUser.IsAdmin)
        {
            throw new ForbiddenException("Chỉ admin mới có quyền xóa sản phẩm");
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }
}
```

---

## Ví dụ sử dụng trong Controller

```csharp
[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        // Exception sẽ được Global Exception Handler bắt và xử lý
        var product = await _productService.GetProductAsync(id);
        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(CreateProductDto dto)
    {
        // Exception sẽ được Global Exception Handler bắt và xử lý
        var product = await _productService.CreateProductAsync(dto);
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        // Get current user từ token
        var currentUser = HttpContext.Items["User"] as User;
        
        if (currentUser == null)
        {
            throw new UnauthorizedException();
        }

        // Exception sẽ được Global Exception Handler bắt và xử lý
        await _productService.DeleteProductAsync(id, currentUser);
        return NoContent();
    }
}
```

---

## Lưu ý

1. **Không cần try-catch trong Controller**: Global Exception Handler sẽ tự động bắt và xử lý tất cả exception.

2. **Throw exception thay vì return error**: Thay vì `return BadRequest("Error")`, hãy `throw new BadRequestException("Error")`.

3. **Message rõ ràng**: Luôn cung cấp message rõ ràng và dễ hiểu cho người dùng.

4. **Logging tự động**: Global Exception Handler sẽ tự động log tất cả exception.

5. **Consistent Error Format**: Tất cả error response đều có format nhất quán với `statusCode`, `message`, `details`, và `timestamp`.
