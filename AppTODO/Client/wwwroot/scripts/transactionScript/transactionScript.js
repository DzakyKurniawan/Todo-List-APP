var table = null;

LoadData();

function clearTextBox() {
    $('#Item').val(0);
    $('#Quantity').val("");
    $('#TotalPrice').val("");
    $('#TransactionDate').val("");


    $('#update').hide();
    $('#submit').show();
    $('#close').show();
}

function Create() {
    if ($('#Item').val() == 0 || $('#Item').val() == " ") {
        Swal.fire("Oops", "Please Choose Item Name", "Error")
    } else if ($('#Quantity').val() == "" || $('#Quantity').val() == " ") {
        Swal.fire("Oops", "Please Insert Quantity", "Error")
    } else if ($('#TransactionDate').val() == "" || $('#TransactionDate').val() == " ") {
        Swal.fire("Oops", "Please Insert TransactionDate", "Error")
    } else {
        debugger;
        var item = new Object();
        item.Quantity = $('#Quantity').val();
        item.TotalPrice = $('#TotalPrice').val();
        item.TransactionDate = $('#TransactionDate').val();
        item.Item = $('#Item').val();

        $.ajax({
            type: "POST",
            url: "/Transaction/Save",
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

function GetByID(ID) {
    debugger;
    $.ajax({
        url: "/Transaction/Getbyid/" + ID,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (result) {

            const obj = JSON.parse(result);
            $('#Id').val(obj.Id);
            $('#Quantity').val(obj.Quantity);
            $('#TotalPrice').val(obj.TotalPrice);
            $('#TransactionDate').val(obj.TransactionDate);
            $('#Item').val(obj.Item);


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
    var transaction = new Object();
    transaction.Id = $('#Id').val();
    transaction.Quantity = $('#Quantity').val();
    transaction.TotalPrice = $('#TotalPrice').val();
    transaction.TransactionDate = $('#TransactionDate').val();
    transaction.Item = $('#Item').val();

    $.ajax({
        type: "PUT",
        url: "/Transaction/Save/",
        data: todo,
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
                url: "/Transaction/Delete",
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

var Items = [];
function LoadItem(element) {
    if (Items.length == 0) {
        $.ajax({
            type: "GET",
            url: "/Item/JsonList",
            success: function (data) {
                Items = data;
                renderItem(element);
            }
        });
    }
    else {
        renderItem(element);
    }
}

function renderItem(element) {
    debugger;
    var $ele = $(element);
    $ele.empty();
    $ele.append($('<option/>').val('0').text('Select Item'));
    $.each(Items, function (i, val) {
        $ele.append($('<option/>').val(val.id).text(val.name));
    });
}

LoadItem($('#Item'));

function LoadData() {
    debugger;
    table = $('#tableTransaction').DataTable({
        'paging': true,
        'serverSide': true,
        'orderMulti':true,
        'ajax': "/Transaction/PageData",
        'columns': [
            {
                data: 'itemName'
            },
            {
                data: 'quantity'
            },
            {
                data: 'totalPrice'
            },
            {
                data: 'transactionDate',
                "render": function (data) {
                    var transdate = moment(data).format('DD/MMMM/YYYY HH:mm');
                    return transdate;
                }
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