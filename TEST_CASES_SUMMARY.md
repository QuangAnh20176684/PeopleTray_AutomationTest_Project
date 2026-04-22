# Stock Catalogue - Comprehensive Test Suite

## 📋 Tóm tắt
File test: `Stock_Catalogue_ComprehensiveTest.cs`

**Tổng cộng: 36 Test Cases** được thiết kế dựa trên khám phá thực tế giao diện từ `https://staging.peopletray.com/Production/Settings#`

---

## 🔍 1. FILTER TESTS (7 test cases)

Kiểm tra tính năng lọc dữ liệu theo Category, Dropdown values, và Clear filters.

| Test Case | Mô tả |
|-----------|-------|
| `Filter_Category_Fuel_DisplaysOnlyFuelItems` | Lọc category "Fuel" - hiển thị chỉ Fuel items |
| `Filter_Category_Consumable_DisplaysOnlyConsumableItems` | Lọc category "Consumable" |
| `Filter_Category_HazardousMaterial_DisplaysOnlyHazardousItems` | Lọc category "Hazardous Material" |
| `Filter_Category_Parts_DisplaysOnlyPartItem` | Lọc category "Parts" |
| `Filter_CategoryDropdown_ShowsAllAvailableOptions` | Verify dropdown có tất cả 7 category |
| `Filter_NoSelection_DisplaysAllCategories` | Không chọn filter - hiển thị tất cả |
| `Filter_ClearFiltersButton_ResetsAllFilters` | Nút "Clear Filters" reset toàn bộ |

---

## 🔎 2. SEARCH TESTS (4 test cases)

Kiểm tra tìm kiếm theo Name, Part Number, kết hợp multiple filters, và Display Inactive toggle.

| Test Case | Mô tả |
|-----------|-------|
| `Search_ByName_ReturnsMatchingItems` | Tìm kiếm theo Name field |
| `Search_ByPartNumber_ReturnsMatchingItems` | Tìm kiếm theo Part Number field |
| `Search_WithCombinedFilters_ReturnsAllMatchingCriteria` | Kết hợp 3 filter: Category + Name + PartNum |
| `Search_DisplayInactiveToggle_ShowsInactiveItems` | Checkbox "Display Inactive" hiển thị inactive items |

---

## ➕ 3. ADD STOCK ITEM TESTS (10 test cases)

Kiểm tra thêm mới stock item, validation rules, và edge cases.

| Test Case | Mô tả |
|-----------|-------|
| `Add_BlankName_ShowsValidationError` | ❌ Name trống → error "This field is required" |
| `Add_BlankUnits_ShowsValidationError` | ❌ Units trống → error |
| `Add_BlankCost_ShowsValidationError` | ❌ Cost trống → error |
| `Add_NonNumericCost_ShowsValidationError` | ❌ Cost không phải số → "Please enter a valid number" |
| `Add_NegativeCost_ShowsValidationError` | ❌ Cost âm → "Cost must be a positive number" |
| `Add_WithValidData_SuccessfullyAddsItem` | ✅ Dữ liệu hợp lệ → Item được add |
| `Add_DuplicateName_ShowsError` | ❌ Name trùng → "{name} already exists" |
| `Add_WithSpecialCharacters_SuccessfullyAdds` | ✅ Name có ký tự đặc biệt (!@#$%) → Add ok |
| `Add_WithZeroCost_SuccessfullyAdds` | ✅ Cost = 0 → Add ok |

---

## ✏️ 4. EDIT TESTS (5 test cases)

Kiểm tra chỉnh sửa các fields của stock item.

| Test Case | Mô tả |
|-----------|-------|
| `Edit_Name_SuccessfullyUpdates` | Edit Name field thành công |
| `Edit_Category_SuccessfullyUpdates` | Edit Category field thành công |
| `Edit_Cost_SuccessfullyUpdates` | Edit Cost field thành công |
| `Edit_PartNumber_SuccessfullyUpdates` | Edit Part Number field thành công |
| `Edit_MultipleFields_InSequence_SuccessfullyUpdates` | Edit nhiều fields liên tiếp |

---

## 🗑️ 5. DELETE TESTS (2 test cases)

Kiểm tra xóa stock item.

| Test Case | Mô tả |
|-----------|-------|
| `Delete_ActiveStock_SuccessfullyDeletes` | Xóa active item → Item biến mất |
| `Delete_InactiveStock_SuccessfullyDeletes` | Xóa inactive item → Item biến mất |

---

## 🔄 6. STATUS CHANGE TESTS (2 test cases)

Kiểm tra chuyển đổi trạng thái Active ↔ Inactive.

| Test Case | Mô tả |
|-----------|-------|
| `Status_ChangeFromActiveToInactive_SuccessfullyUpdates` | Đổi Active → Inactive → Item ẩn |
| `Status_ChangeFromInactiveToActive_SuccessfullyUpdates` | Đổi Inactive → Active → Item hiện |

---

## 🎯 Các UI Elements được test

### Filter Panel (Bên trái)
- ✅ Category dropdown (7 options)
- ✅ Name textbox
- ✅ Part Number textbox
- ✅ Display Inactive checkbox
- ✅ Search button
- ✅ Clear Filters link

### Stock Catalogue Table (Bên phải)
- ✅ Category column
- ✅ Name column
- ✅ Part Number column
- ✅ Units column
- ✅ Cost column
- ✅ Description column
- ✅ Sub Types column
- ✅ Edit/Delete buttons

### Action Buttons (Top)
- ✅ Add Stock Item button
- ✅ Edit Subtypes button
- ✅ Export Stock Catalogue button

---

## 📊 Test Execution

### Chạy tất cả tests:
```bash
dotnet test TestX.sln --filter "Stock_Catalogue_ComprehensiveTest"
```

### Chạy theo category:
```bash
# Chỉ filter tests
dotnet test TestX.sln --filter "Category=Filter"

# Chỉ search tests
dotnet test TestX.sln --filter "Category=Search"

# Chỉ add tests
dotnet test TestX.sln --filter "Category=Add"

# Chỉ edit tests
dotnet test TestX.sln --filter "Category=Edit"
```

---

## 🔗 File Location
```
/Users/nguyenquanganh/Peo_Test/src/testSuit/ProductionTest/SettingTest/Production_Settings/
├── Stock_Catalogue_CategoryFilterTest.cs       (Original - Deprecated)
└── Stock_Catalogue_ComprehensiveTest.cs        (New - 36 Test Cases)
```

---

## 📝 Notes

1. Tất cả tests đều sử dụng base class `CommonBaseTest` từ project
2. Sử dụng `StockCataloguePage` page object model
3. Mỗi test tự động:
   - Setup: Login + Navigate to Settings → Stock Catalogue
   - Cleanup: Implicit via NUnit teardown
4. Random data generation sử dụng `generateHelper.GenerateRandomString()`
5. Validation assertions rõ ràng với error messages chi tiết
