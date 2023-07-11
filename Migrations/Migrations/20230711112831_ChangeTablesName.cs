using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Academits.Karetskas.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTablesName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_categoryProduct_category_CategoriesId",
                table: "categoryProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_categoryProduct_product_ProductsId",
                table: "categoryProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_order_customer_CustomerId",
                table: "order");

            migrationBuilder.DropForeignKey(
                name: "FK_orderItem_order_OrderId",
                table: "orderItem");

            migrationBuilder.DropForeignKey(
                name: "FK_orderItem_product_ProductId",
                table: "orderItem");

            migrationBuilder.DropForeignKey(
                name: "FK_product_Suppliers_SupplierId",
                table: "product");

            migrationBuilder.DropPrimaryKey(
                name: "PK_categoryProduct",
                table: "categoryProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_product",
                table: "product");

            migrationBuilder.DropPrimaryKey(
                name: "PK_orderItem",
                table: "orderItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_order",
                table: "order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_customer",
                table: "customer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_category",
                table: "category");

            migrationBuilder.RenameTable(
                name: "categoryProduct",
                newName: "CategoryProduct");

            migrationBuilder.RenameTable(
                name: "product",
                newName: "Products");

            migrationBuilder.RenameTable(
                name: "orderItem",
                newName: "OrdersItems");

            migrationBuilder.RenameTable(
                name: "order",
                newName: "Orders");

            migrationBuilder.RenameTable(
                name: "customer",
                newName: "Customers");

            migrationBuilder.RenameTable(
                name: "category",
                newName: "Categories");

            migrationBuilder.RenameColumn(
                name: "ProductsId",
                table: "CategoryProduct",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "CategoriesId",
                table: "CategoryProduct",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_categoryProduct_ProductsId",
                table: "CategoryProduct",
                newName: "IX_CategoryProduct_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_product_SupplierId",
                table: "Products",
                newName: "IX_Products_SupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_orderItem_ProductId",
                table: "OrdersItems",
                newName: "IX_OrdersItems_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_orderItem_OrderId",
                table: "OrdersItems",
                newName: "IX_OrdersItems_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_order_CustomerId",
                table: "Orders",
                newName: "IX_Orders_CustomerId");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CategoryProduct",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryProduct",
                table: "CategoryProduct",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrdersItems",
                table: "OrdersItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customers",
                table: "Customers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryProduct_CategoryId",
                table: "CategoryProduct",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryProduct_Categories_CategoryId",
                table: "CategoryProduct",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryProduct_Products_ProductId",
                table: "CategoryProduct",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Customers_CustomerId",
                table: "Orders",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersItems_Orders_OrderId",
                table: "OrdersItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersItems_Products_ProductId",
                table: "OrdersItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Suppliers_SupplierId",
                table: "Products",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryProduct_Categories_CategoryId",
                table: "CategoryProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryProduct_Products_ProductId",
                table: "CategoryProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customers_CustomerId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdersItems_Orders_OrderId",
                table: "OrdersItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdersItems_Products_ProductId",
                table: "OrdersItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Suppliers_SupplierId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryProduct",
                table: "CategoryProduct");

            migrationBuilder.DropIndex(
                name: "IX_CategoryProduct_CategoryId",
                table: "CategoryProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrdersItems",
                table: "OrdersItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customers",
                table: "Customers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CategoryProduct");

            migrationBuilder.RenameTable(
                name: "CategoryProduct",
                newName: "categoryProduct");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "product");

            migrationBuilder.RenameTable(
                name: "OrdersItems",
                newName: "orderItem");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "order");

            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "customer");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "category");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "categoryProduct",
                newName: "ProductsId");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "categoryProduct",
                newName: "CategoriesId");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryProduct_ProductId",
                table: "categoryProduct",
                newName: "IX_categoryProduct_ProductsId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_SupplierId",
                table: "product",
                newName: "IX_product_SupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_OrdersItems_ProductId",
                table: "orderItem",
                newName: "IX_orderItem_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_OrdersItems_OrderId",
                table: "orderItem",
                newName: "IX_orderItem_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_CustomerId",
                table: "order",
                newName: "IX_order_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_categoryProduct",
                table: "categoryProduct",
                columns: new[] { "CategoriesId", "ProductsId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_product",
                table: "product",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_orderItem",
                table: "orderItem",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_order",
                table: "order",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_customer",
                table: "customer",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_category",
                table: "category",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_categoryProduct_category_CategoriesId",
                table: "categoryProduct",
                column: "CategoriesId",
                principalTable: "category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_categoryProduct_product_ProductsId",
                table: "categoryProduct",
                column: "ProductsId",
                principalTable: "product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_order_customer_CustomerId",
                table: "order",
                column: "CustomerId",
                principalTable: "customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_orderItem_order_OrderId",
                table: "orderItem",
                column: "OrderId",
                principalTable: "order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_orderItem_product_ProductId",
                table: "orderItem",
                column: "ProductId",
                principalTable: "product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_product_Suppliers_SupplierId",
                table: "product",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id");
        }
    }
}
