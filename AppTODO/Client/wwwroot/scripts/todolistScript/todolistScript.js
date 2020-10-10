var table = null;

$('#status').change(function () {
    debugger;
    table.ajax.url('/ToDo/PageData/' + $('#status').val()).load();
});

LoadData();

function clearTextBox() {
    $('#ToDo').val("");

    $('#update').hide();
    $('#submit').show();
    $('#close').show();
}

function Create() {
    if ($('#ToDo').val() == "" || $('#ToDo').val() == " ") {
        Swal.fire("Oops", "Please Insert To Do List ", "error")       
    } else {
        debugger;
        var todo = new Object();
        todo.TodoName = $('#ToDo').val();

        $.ajax({
            type: "POST",
            url: "/ToDo/Save",
            data: todo,
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
                url: "/ToDo/Delete",
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

function GetbyID(ID) {
    debugger;
    $.ajax({
        url: "/ToDo/Get/" + ID,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (result) {

            const obj = JSON.parse(result);
            $('#Id').val(obj.Id);
            $('#ToDo').val(obj.ToDoName);

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
    var todo = new Object();
    todo.Id = $('#Id').val();
    todo.TodoName = $('#ToDo').val();

    $.ajax({
        type: "POST",
        url: "/ToDo/Save/",
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

function UpdateStatus(Id) {
    debugger;
    $.ajax({
        url: "/ToDo/UpdateStatus",
        type: "DELETE",
        data: { id: Id },
        success: function (result) {
            alert('Completed');
            table.ajax.reload();
        },
        error: function (result) {
            alert('error');
            table.ajax.reload();
        }
    });
}

function LoadData() {
    table = $('#tableToDolist').DataTable({
        'paging':true,
        'serverSide': true,
        'ajax': "/ToDo/PageData/" + $('#status').val(),
        'columns': [
            {
                data: 'status',
                "render": function (data, type, row) {
                    console.log(row.id);
                    if (data) {
                        return '<a href="#" class="disabled"><i class="fa fa-check-circle" ></i></a>';
                    }
                    return '<a href="#" onclick="UpdateStatus(' + row.id + ')"><i class="fa fa-check-circle"></i></a>';
                }
            },
            {
                data: 'todoName'
            },
            {
                data: 'status',
                "render": function (data) {
                    if (data) {
                        return 'Completed';
                    }
                    return 'Active';
                }
            },
            {
                data: 'createDate',
                "render": function (data) {
                    var createdate = moment(data).format('DD/MMMM/YYYY HH:mm');
                    return createdate;
                }
            },
            {
                data: 'status',
                "render": function (data, type, row) {
                    if (data) {
                        return '<a href="#" class="disabled"><i class="fa fa-edit" ></i></a> | <a href="#" class="disabled"><i class="fa fa-trash"></i></a>';
                    }
                    return '<a href="#" onclick="return GetbyID(' + row.id + ')"><i class="fa fa-edit"></i></a> | <a href="#" onclick="Delete(' + row.id + ')"><i class="fa fa-trash"></i></a>';
                }
            }
        ]
    });
}