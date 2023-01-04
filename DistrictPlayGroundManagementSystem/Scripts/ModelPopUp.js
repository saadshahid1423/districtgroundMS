jQuery(document).ready(function () {
    $.ajaxSetup({ cache: false });
    $(".data_modal").on("click", function (e) {
        $('#myModalContent').load(this.href, function () {
            $('#myModal').modal({
                keyboard: false
            }, 'show');
            bindForm(this);
        });
        return false;
    });
});

function OpenModelPopup(elem, callback) {

    $.ajaxSetup({ cache: false });
    var href = elem.attributes.data.nodeValue;
    $('#myModalContent').html('<div class="load2 load-wrapper"><div class="loader">Loading...</div></div>');
    $('#myModal').modal({}, 'show');
    $('#myModalContent').load(href, function () {
        bindForm(this, callback);
    });
}
var x;
function bindForm(dialog, callback) {
    $('.form', dialog).submit(function () {
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                if (callback) {
                    callback(result);
                }
                else {
                    if (result.success) {
                        var id = "replacetarget";
                        //x = result.row;
                        //t.row.add(result.row).draw(false);

                        $('#' + id).html('<div class="lds-ripple"><div class="lds-pos"></div><div class="lds-pos"></div></div>');
                        $('#myModal').modal('hide');
                        if (result.index) {
                            id += result.index;
                        }
                        $('#' + id).load(result.url, function () {

                            $('#dataTables-example').DataTable({
                                responsive: true,
                                pageLength: 10,
                                sPaginationType: "full_numbers",
                                oLanguage: {
                                    oPaginate: {
                                        sFirst: "<<",
                                        sPrevious: "<",
                                        sNext: ">",
                                        sLast: ">>"
                                    }
                                }
                            });
                            if (result.callback != null) {
                                GetList();
                            }
                            $('#DivMessage').html('<i class="fa fa-check"></i> ' + result.message);
                            $('#DivMessage').addClass('notes notes-success text-center');
                            $('#DivMessage').fadeIn();
                        });
                    } else {
                        $('#divMessage').html('<i class="fa fa-warning"></i> ' + result.message);
                        $('#divMessage').fadeIn();
                    }
                }
            }
        });
        return false;
    });
}

//function GetFormatedObject(row) {
//    $('#addRow').on('click', function () {
//        t.row.add([
//            counter + '.1',
//            counter + '.2',
//            counter + '.3',
//            counter + '.4',
//            counter + '.5'
//        ]).draw(false);

//        counter++;
//    });
//}