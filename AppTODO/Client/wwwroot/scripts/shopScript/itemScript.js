var table = null;

LoadData();

function clearTextBox() {
    $('#Name').val("");
    $('#Price').val("");
    $('#Stock').val("");
    $('#Supplier').val(0);


    $('#update').hide();
    $('#submit').show();
    $('#close').show();
}

function Create() {
    if ($('#Name').val() == "" || $('#Name').val() == " ") {
        Swal.fire("Oops", "Please Insert Item Name ", "error");
    } else if ($('#Price').val() == "" || $('#Price').val() == " ") {
        Swal.fire("Oops", "Please Insert Item Price ", "error");
    } else if ($('#Stock').val() == "" || $('#Stock').val() == " ") {
        Swal.fire("Oops", "Please Insert Item Price ", "error");
    } else {
        debugger;
        var item = new Object();
        item.Name = $('#Name').val();
        item.Price = $('#Price').val();
        item.Stock = $('#Stock').val();
        item.Supplier = $('#Supplier').val();

        $.ajax({
            type: "POST",
            url: "/Item/Save",
            data: item,
            success: function (result) {
                Swal.fire({
                    position: 'center',
                    type: 'success',
                    title: 'Create Successfully'
                });
                table.ajax.reload();
                $('#myModal').modal('hide');
            },
            error: function (result) {
                Swal.fire('center', 'error', 'Create Fail');
                table.ajax.reload();
            }
        });
    }
}

function GetbyID(Id) {
    debugger;
    $.ajax({
        url: "/Item/Getby/" + Id,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (result) {

            const obj = JSON.parse(result);
            $('#Id').val(obj.Id);
            $('#Name').val(obj.Name);
            $('#Price').val(obj.Price);
            $('#Stock').val(obj.Stock);
            $('#Supplier').val(obj.Supplier);

            $('#myModal').modal('show');
            $('#update').show();
            $('#submit').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function Update() {
    var item = new Object();
    item.Id = $('#Id').val();
    item.Name = $('#Name').val();
    item.Price = $('#Price').val();
    item.Stock = $('#Stock').val();
    item.Supplier = $('#Supplier').val();

    $.ajax({
        type: "POST",
        url: "/Item/Save/",
        data: item,
        success: function (result) {
            Swal.fire({
                position: 'center',
                type: 'success',
                title: 'Update Successfully'
            });
            table.ajax.reload();
        },
        error: function (result) {
            Swal.fire('center', 'error', 'Update Fail');
        }
    });
}

function Delete(Id) {
    debugger;
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                url: "/Item/Delete",
                type: "DELETE",
                data: { id: Id },
                success: function (result) {
                    Swal.fire({
                        position: 'center',
                        type: 'success',
                        title: 'Delete Successfully'
                    });
                    table.ajax.reload();
                },
                error: function (result) {
                    Swal.fire('center', 'error', 'Delete Fail');
                    table.ajax.reload();
                }
            });
        }
    });
}

var Suppliers = [];
function LoadSupplier(element) {
    if (Suppliers.length == 0) {
        $.ajax({
            type: "GET",
            url: "/Supplier/JsonList",
            success: function (data) {
                Suppliers = data;
                renderSupplier(element);
            }
        });
    }
    else {
        renderSupplier(element);
    }
}

function renderSupplier(element) {
    debugger;
    var $ele = $(element);
    $ele.empty();
    $ele.append($('<option/>').val('0').text('Select Supplier'));
    $.each(Suppliers, function (i, val) {
        $ele.append($('<option/>').val(val.id).text(val.name));
    });
}

LoadSupplier($('#Supplier'));

function LoadData() {
    debugger;
    table = $('#tableItem').DataTable({
        'paging': true,
        'serverSide': true,
        'ajax': "/Item/PageData",
        'columns': [
            {
                data: 'name'
            },
            {
                data: 'price'
            },
            {
                data: 'stock'
            },
            {
                data: 'id',
                "render": function (data) {
                    return '<a href="#" onclick="return GetbyID(' + data + ')"><i class="fa fa-edit"></i></a> | <a href="#" onclick="Delete(' + data + ')"><i class="fa fa-trash"></i></a>';
                }
            }
        ]
    });
}