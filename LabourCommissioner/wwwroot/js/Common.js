const ResponseMsgType = {
    success: 1,
    error: 2,
    warning: 3,
    info: 4
}

function ShowDynamicSwalAlert(title, msg) {
    const myArray = msg.split("||");
    msg = myArray[0];
    var type = myArray[1];
    if (msg != null && msg != '') {
        if (type.toString() == ResponseMsgType.success.toString()) {
            debugger;
            swal({
                title: title,
                text: msg,
                type: "success",
                showCancelButtonClass: "btn-primary",
                confirmButtonText: 'OK'
                //}, function () {
                //    window.location = '/home/ApplicationDetails?ApplicationId=7';
            });
        }
        else if (type.toString() == ResponseMsgType.error.toString()) {
            swal({
                title: title,
                text: msg,
                type: "error",
                showCancelButtonClass: "btn-primary",
                confirmButtonText: 'OK'
            });
        }
        else if (type.toString() == ResponseMsgType.warning.toString()) {
            swal({
                title: title,
                text: msg,
                type: "warning",
                showCancelButtonClass: "btn-primary",
                confirmButtonText: 'OK'
            });
        }
        else if (type.toString() == ResponseMsgType.info.toString()) {
            swal({
                title: title,
                text: msg,
                type: "info",
                showCancelButtonClass: "btn-primary",
                confirmButtonText: 'OK'
            });
        }
    }
}

function LoadSchemePartialView(controllerName, actionName, ServiceId, ApplicationId, isFilled) {
    debugger;
    const ul = document.getElementById('dvtablist');
    const listItems = ul.getElementsByTagName('li');
    // Loop through the NodeList object.

    for (let i = 0; i <= listItems.length - 1; i++) {
        // console.log(listItems[i]);
        var oChild = listItems[i].children;
        var childeleId = oChild[0].id;
        $("#" + childeleId).removeClass('active');
    }
    if (actionName != "") {
        $("#" + actionName).addClass('active');
    }
    $.ajax({
        type: "GET",
        url: "/" + controllerName + "/" + actionName,
        data: { ServiceId: ServiceId, strApplicationId: ApplicationId, isFilled: isFilled },
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        /*dataType: "html",*/
        /* dataType: 'JSON',*/
        success: function (response) {
            $('#dvTabPartialView').html('');
            $('#dvTabPartialView').html(response);
            $("form").removeData("validator").removeData("unobtrusiveValidation");
            $.validator.unobtrusive.parse($("form"));
        },
        failure: function (response) {
            window.location = "/Home/Error"
        },
        error: function (response) {
            window.location = "/Home/Error"
        }
    });
}


function LoadSchemeHTYClaimPartialView(controllerName, actionName, ServiceId, ApplicationId, isFilled, ClaimApplicationId) {
    debugger;
    const ul = document.getElementById('dvtablist');
    const listItems = ul.getElementsByTagName('li');
    // Loop through the NodeList object.

    for (let i = 0; i <= listItems.length - 1; i++) {
        // console.log(listItems[i]);
        var oChild = listItems[i].children;
        var childeleId = oChild[0].id;
        $("#" + childeleId).removeClass('active');
    }
    if (actionName != "") {
        $("#" + actionName).addClass('active');
    }
    $.ajax({
        type: "GET",
        url: "/" + controllerName + "/" + actionName,
        data: { ServiceId: ServiceId, strApplicationId: ApplicationId, isFilled: isFilled, strClaimApplicationId: ClaimApplicationId },
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        /*dataType: "html",*/
        /* dataType: 'JSON',*/
        success: function (response) {
            $('#dvTabPartialView').html('');
            $('#dvTabPartialView').html(response);
            $("form").removeData("validator").removeData("unobtrusiveValidation");
            $.validator.unobtrusive.parse($("form"));
        },
        failure: function (response) {
            window.location = "/Home/Error"
        },
        error: function (response) {
            window.location = "/Home/Error"
        }
    });
}
function GetDistrict() {
    debugger;
    $.ajax({
        type: "get",
        url: "/BOCWSikshanSahayYojana/GetDistrict",
        //data: { DepartmentId: $('#DepartmentId').val() },
        datatype: "json",
        traditional: true,
        success: function (data) {
            debugger;
            var DistrictList = "";
            DistrictList = DistrictList + '<option value="">--Select--</option>';
            for (var i = 0; i < data.data.result.length; i++) {
                DistrictList = DistrictList + '<option value=' + data.data.result[i].value + '>' + data.data.result[i].text + '</option>';
            }
            $('#listDistrict').html(DistrictList);
            //$('#PlistDistrict').html(DistrictList);
        }
    });
}

function LoadPaymentPartialView(controllerName, actionName, fromDate, toDate) {
    debugger;

    //const ul = document.getElementById('dvtablist');
    //const listItems = ul.getElementsByTagName('li');
    // Loop through the NodeList object.

    //for (let i = 0; i <= listItems.length - 1; i++) {
    //    // console.log(listItems[i]);
    //    var oChild = listItems[i].children;
    //    var childeleId = oChild[0].id;
    //    $("#" + childeleId).removeClass('active');
    //}
    //if (actionName != "") {
    //    $("#" + actionName).addClass('active');
    //}
    $.ajax({
        type: "GET",
        url: "/" + controllerName + "/" + actionName,
        data: { fromDate: fromDate, toDate: toDate },
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        /*dataType: "html",*/
        /* dataType: 'JSON',*/
        success: function (response) {
            $('#dvPaymentPartialView').html('');
            $('#dvPaymentPartialView').html(response);
            //$("form").removeData("validator").removeData("unobtrusiveValidation");
            //$.validator.unobtrusive.parse($("form"));
        },
        failure: function (response) {
            window.location = "/Home/Error"
        },
        error: function (response) {
            window.location = "/Home/Error"
        }
    });
}



var districtID;
function GetDistrictByDivisionId(divisionId) {
    districtID = districtId;
    $.ajax({
        type: "get",
        url: "/BOCWSikshanSahayYojana/GetTalukaByDistrictId",
        data: { districtId: districtId },
        datatype: "json",
        traditional: true,
        success: function (data) {
            debugger;
            var TalukaList = "";
            console.log(data.data.result.length);
            TalukaList = TalukaList + '<option value="">--Select--</option>';
            for (var i = 0; i < data.data.result.length; i++) {
                TalukaList = TalukaList + '<option value=' + data.data.result[i].value + '>' + data.data.result[i].text + '</option>';
            }
            $('#listTaluka').html(TalukaList);
        }
    });
}


var districtID;
function GetTalukaByDistrictId(districtId) {
    districtID = districtId;
    $.ajax({
        type: "get",
        url: "/BOCWSikshanSahayYojana/GetTalukaByDistrictId",
        data: { districtId: districtId },
        datatype: "json",
        traditional: true,
        success: function (data) {
            debugger;
            var TalukaList = "";
            console.log(data.data.result.length);
            TalukaList = TalukaList + '<option value="">- Please Select -</option>';
            for (var i = 0; i < data.data.result.length; i++) {
                TalukaList = TalukaList + '<option value=' + data.data.result[i].value + '>' + data.data.result[i].text + '</option>';
            }
            $('#listTaluka').html(TalukaList);
        }
    });
}

function GetRole(districtId) {
    debugger;
    districtID = districtId;
    $.ajax({
        type: "get",
        url: "/EmployeeMaster/GetRole",
        data: { districtId: districtId },
        datatype: "json",
        traditional: true,
        success: function (data) {
            debugger;
            var RoleList = "";
            console.log(data.data.result.length);
            RoleList = RoleList + '<option value="">- Please Select -</option>';
            for (var i = 0; i < data.data.result.length; i++) {
                RoleList = RoleList + '<option value=' + data.data.result[i].value + '>' + data.data.result[i].text + '</option>';
            }
            $("#listRole").html(RoleList);
        }
    });
}


var Benifitsrs = "";
var Hostelbenifits = "";
var Bookbenifits = "";
var UserBenifitsrs = "";
var UserHostelbenifits = "";
var UserBookbenifits = "";
var Finalbenifits = "";
var Finalhostelbenifits = "";
var Finalbookbenifits = "";
var Totalsahay = "";
var minBenifit = "";
var maxBenifit = "";

/*var courseid;*/
function GetSemesterbyCourseId(courseId) {
    debugger;
    /*    courseid = courseId;*/
    debugger;
    $.ajax({
        type: "get",
        url: "/BOCWSikshanSahayYojana/GetSemesterbyCourseId",
        data: { courseId: courseId },
        datatype: "json",
        traditional: true,
        success: function (data) {
            debugger;
            var Sem = "";
            console.log(data.data.result.length);
            Sem = Sem + '<option value="">--Select--</option>';
            for (var i = 0; i < data.data.result.length; i++) {
                Sem = Sem + '<option value=' + data.data.result[i].value + '>' + data.data.result[i].text + '</option>';
            }

            $('#AcadmicYearSem').val("");
            //$('#AcadmicYearSem').val(Sem);
            $("#AcadmicYearSem").html(Sem);

        }
    });
}


//function GetBenifitByCourseId(semesteryear, courseid) {
//    $.ajax({
//        type: "get",
//        url: "/BOCWSikshanSahayYojana/GetBenifitByCourseId",
//        data: { courseId: courseid, semesteryear: semesteryear },
//        datatype: "json",
//        traditional: true,
//        success: function (data) {
//            debugger;

//            $('#Finalbenifits').val(0);
//            $('#Finalhostelbenifits').val(0);
//            $('#Finalbookbenifits').val(0);
//            var val = $("#Syllabus").val();

//            if (val == 6 || val == 7 || val == 8 || val == 9) {
//                minBenifit = data.data.result[0].minfees;
//                maxBenifit = data.data.result[0].maxfees;
//                Benifitsrs = minBenifit;
//            }

//            //minBenifit = data.data.result[0].minfees;
//            //maxBenifit = data.data.result[0].maxfees;
//            Benifitsrs = data.data.result[0].benifitsrs;
//            Hostelbenifits = data.data.result[0].hostelbenifits;
//            Bookbenifits = data.data.result[0].booksbenifits;
//            UserBenifitsrs = data.data.result[0].ubenifitsrs;
//            UserHostelbenifits = data.data.result[0].uhostelbenifits;
//            UserBookbenifits = data.data.result[0].ubooksbenifits;
//            Finalbenifits = data.data.result[0].fbenifitsrs;
//            Finalhostelbenifits = data.data.result[0].fhostelbenifits;
//            Finalbookbenifits = data.data.result[0].fbooksbenifits;
//            $('#Benifitsrs').val(Benifitsrs);
//            $('#Hostelbenifits').val(Hostelbenifits);
//            $('#Bookbenifits').val(Bookbenifits);
//            $('#Userbenifitsrs').val(UserBenifitsrs);
//            $('#Userhostelbenifits').val(UserHostelbenifits);
//            $('#Userbookbenifits').val(UserBookbenifits);
//            //$('#Finalbenifits').val(Benifitsrs);
//            //$('#Finalhostelbenifits').val(Hostelbenifits);
//            //$('#Finalbookbenifits').val(Bookbenifits);
//            /* $('#Benifitsrs').attr('disabled', 'disabled');*/
//            //$('#PlistTaluka').html(TalukaList);
//        }
//    });
//}



function GetFinalBenifits(feesOf) {
    debugger;
    GetBenifitByCourseId($("#AcadmicYearSem").val(), $('#GSubject').val(), 1);
    var val = $("#GSyllabus").val();
    debugger;
    //if (val == 4 || val == 5 || val == 6 || val == 7 || val == 8 || val == 9) {
    if (val == 6 || val == 7 || val == 8) {
        debugger;
        if (feesOf == 'School') {
            debugger;
            if (parseInt($('#Userbenifitsrs').val()) <= parseInt($('#Benifitsrs').val())) {
                $('#Finalbenifits').val(0);
                $('#Finalbenifits').val($('#Benifitsrs').val());

            }
            else if (parseInt($('#Userbenifitsrs').val()) > parseInt($('#Benifitsrs').val()) &&
                parseInt($('#Userbenifitsrs').val()) < parseInt(maxBenifit)) {
                $('#Finalbenifits').val(0);
                $('#Finalbenifits').val($('#Userbenifitsrs').val());
                $('#Benifitsrs').val(parseInt(maxBenifit));

            } else {
                $('#Finalbenifits').val(0);
                $('#Finalbenifits').val(parseInt(maxBenifit));
                $('#Benifitsrs').val(parseInt(maxBenifit));

            }

        }
        else if (feesOf == 'Hostel') {
            debugger;
            if (parseInt($('#Userhostelbenifits').val()) < parseInt($('#Hostelbenifits').val())) {
                $('#Finalhostelbenifits').val(0);
                $('#Finalhostelbenifits').val($('#Userhostelbenifits').val());

            }
            else {
                $('#Finalhostelbenifits').val($('#Hostelbenifits').val());

            }
        }
        else if (feesOf == 'books') {
            debugger;
            if (parseInt($('#Userbookbenifits').val()) < parseInt($('#Bookbenifits').val())) {
                $('#Finalbookbenifits').val(0);
                $('#Finalbookbenifits').val($('#Userbookbenifits').val());

            }
            else {
                $('#Finalbookbenifits').val($('#Bookbenifits').val());
            }
        }

    }
    else {
        if (feesOf == 'School') {
            $('#Finalbenifits').val(Benifitsrs);
        }
        if (feesOf == 'Hostel') {
            if (parseInt($('#Hostelbenifits').val()) == 0) {
                $('#Userhostelbenifits').val(0);
                $('#Finalhostelbenifits').val($('#Hostelbenifits').val());

            } else {
                if (parseInt($('#Userhostelbenifits').val()) < parseInt($('#Hostelbenifits').val())) {
                    $('#Finalhostelbenifits').val(0);
                    $('#Finalhostelbenifits').val($('#Userhostelbenifits').val());

                } else {
                    $('#Finalhostelbenifits').val($('#Hostelbenifits').val());

                }
            }
        }
        if (feesOf == 'books') {
            if (parseInt($('#Bookbenifits').val()) == 0) {
                $('#Userbookbenifits').val(0);
                $('#Finalbookbenifits').val($('#Bookbenifits').val());

            } else {
                if (parseInt($('#Userbookbenifits').val()) < parseInt($('#Bookbenifits').val())) {
                    $('#Finalbookbenifits').val(0);
                    $('#Finalbookbenifits').val($('#Userbookbenifits').val());

                } else {
                    $('#Finalbookbenifits').val($('#Bookbenifits').val());
                }
            }

        }

    }
    var totalsahay = parseInt($('#Finalbenifits').val()) + parseInt($('#Finalhostelbenifits').val()) + parseInt($('#Finalbookbenifits').val());
    $('#Totalsahay').val(totalsahay);
}



function GetVillageByDistrictIdAndTalukaId(talukaId) {
    var districtId = districtID;
    $.ajax({
        type: "get",
        url: "/BOCWSikshanSahayYojana/GetVillageByDistrictIdAndTalukaId",
        data: { districtId: districtId, talukaId: talukaId },
        datatype: "json",
        traditional: true,
        success: function (data) {
            var VillageList = "";
            console.log(data.data.result.length);
            VillageList = VillageList + '<option value="">- Please Select -</option>';
            for (var i = 0; i < data.data.result.length; i++) {
                VillageList = VillageList + '<option value=' + data.data.result[i].value + '>' + data.data.result[i].text + '</option>';
            }
            $('#listVillage').html(VillageList);
            //$('#PlistVillage').html(VillageList);
        }
    });
}

function GetEVillageByDistrictIdAndTalukaId(districtid, talukaId) {

    $.ajax({
        type: "get",
        url: "/BOCWSikshanSahayYojana/GetVillageByDistrictIdAndTalukaId",
        data: { districtId: districtid, talukaId: talukaId },
        datatype: "json",
        traditional: true,
        success: function (data) {
            debugger;
            var VillageList = "";
            console.log(data.data.result.length);
            VillageList = VillageList + '<option value="0">-- ALL --</option>';
            for (var i = 0; i < data.data.result.length; i++) {
                VillageList = VillageList + '<option value=' + data.data.result[i].value + '>' + data.data.result[i].text + '</option>';
            }
            $('#villageId').html(VillageList);
            //$('#PlistVillage').html(VillageList);
        }
    });
}


//function GetSemesterbyCourseId(courseId) {
//    $.ajax({
//        type: "get",
//        url: "/BOCWSikshanSahayYojana/GetSemesterbyCourseId",
//        data: { courseId: courseId },
//        datatype: "json",
//        traditional: true,
//        success: function (data) {
//            debugger;
//            var Sem = "";
//            console.log(data.data.result.length);
//            Sem = Sem + '<option value="">--Select--</option>';
//            for (var i = 0; i < data.data.result.length; i++) {
//                Sem = Sem + '<option value=' + data.data.result[i].value + '>' + data.data.result[i].text + '</option>';
//            }


//            $('#GSubject').val(Semesteryear);
//        }
//    });
// }       

function GetPDistrict(stateid) {
    var val = $("#PlistState").val();
    if (val != 7) {
        //$('#PlistDistrict').replaceWith($('<input />', { 'type': 'text', 'value': 'Other' }));
        $('#dvGuj').hide();
        $('#dvWithoutGuj').show();
        $('#dvGujTaluka').hide();
        $('#dvWithoutGujTaluka').show();
        $('#dvGujVillage').hide();
        $('#dvWithoutGujVillage').show();

        $('#PlistDistrict').val('0');
        $('#PlistTaluka').val('0');
        $('#PlistVillage').val('0');
    }
    else {
        $('#dvWithoutGuj').hide();
        $('#dvGuj').show();
        $('#dvGujTaluka').show();
        $('#dvWithoutGujTaluka').hide();
        $('#dvGujVillage').show();
        $('#dvWithoutGujVillage').hide();
        $("#PDistrictNameInEng").val('');
        $('#PTalukaNameInEng').val('');
        $('#PVillageNameInEng').val('');

        //GetPDistrict();

    }
    $.ajax({
        type: "get",
        url: "/BOCWSikshanSahayYojana/GetDistrict",
        //data: {DepartmentId: $('#DepartmentId').val() },
        datatype: "json",
        traditional: true,
        async: false,
        success: function (data) {
            if (stateid == 7) {
                var DistrictList = "";
                DistrictList = DistrictList + '<option value="">--Select--</option>';
                for (var i = 0; i < data.data.result.length; i++) {
                    DistrictList = DistrictList + '<option value=' + data.data.result[i].value + '>' + data.data.result[i].text + '</option>';
                }
                //$('#listDistrict').html(DistrictList);
                $('#PlistDistrict').html(DistrictList);
                $('#PlistTaluka').html('<option value="">--Select--</option>');
                $('#PlistVillage').html('<option value="">--Select--</option>');
            }
        }
    });
}



function bindservicemaster(beneficiarytypeid) {
    debugger;
    beneficiarytypeid = beneficiarytypeid;
    $.ajax({
        type: "get",
        url: "/Masters/bindservicemaster",
        data: { beneficiarytypeid: beneficiarytypeid },
        datatype: "json",
        traditional: true,
        success: function (data) {
            debugger;
            var ServicesList = "";
            console.log(data.data.result.length);
            ServicesList = ServicesList + '<option value="">- Please Select -</option>';
            for (var i = 0; i < data.data.result.length; i++) {
                ServicesList = ServicesList + '<option value=' + data.data.result[i].value + '>' + data.data.result[i].text + '</option>';
            }
            $("#lstservicenames").html(ServicesList);
        }
    });
}

var PdistrictID;
function GetPTalukaByDistrictId(districtId) {
    PdistrictID = districtId;
    $.ajax({
        type: "get",
        url: "/BOCWSikshanSahayYojana/GetTalukaByDistrictId",
        data: { districtId: districtId },
        datatype: "json",
        traditional: true,
        async: false,
        success: function (data) {
            var TalukaList = "";
            console.log(data.data.result.length);
            TalukaList = TalukaList + '<option value="">--Select--</option>';
            for (var i = 0; i < data.data.result.length; i++) {
                TalukaList = TalukaList + '<option value=' + data.data.result[i].value + '>' + data.data.result[i].text + '</option>';
            }
            //$('#listTaluka').html(TalukaList);
            $('#PlistTaluka').html(TalukaList);
        }
    });
}

function GetPVillageByDistrictIdAndTalukaId(talukaId) {
    var districtId = PdistrictID;
    $.ajax({
        type: "get",
        url: "/BOCWSikshanSahayYojana/GetVillageByDistrictIdAndTalukaId",
        data: { districtId: districtId, talukaId: talukaId },
        datatype: "json",
        traditional: true,
        async: false,
        success: function (data) {
            var VillageList = "";
            console.log(data.data.result.length);
            VillageList = VillageList + '<option value="">--Select--</option>';
            for (var i = 0; i < data.data.result.length; i++) {
                VillageList = VillageList + '<option value=' + data.data.result[i].value + '>' + data.data.result[i].text + '</option>';
            }
            //$('#listVillage').html(VillageList);
            $('#PlistVillage').html(VillageList);
        }
    });
}

$(document).ready(function () {


});
function ValidateDocumentFile() {

    $(".fileupload").on("change", function (event) {
        debugger;
        const file = event.target.files[0]
        const filereader = new FileReader()

        filereader.readAsArrayBuffer(file);

        filereader.onloadend = function () {
            debugger;
            var arrayBuffer = filereader.result
            //var bytes = new Uint8Array(arrayBuffer);
            //console.log(bytes);

            let fileSize = event.currentTarget.getAttribute('data-size')
            if (fileSize == null || fileSize == 0) {
                fileSize = 1;
            }
            var chkFileSize = 1000000 * fileSize;
            //const uint = new Uint8Array(arrayBuffer)
            const uint = (new Uint8Array(arrayBuffer)).subarray(0, 4);
            let bytes = []
            uint.forEach((byte) => {
                bytes.push(byte.toString(16))
            })
            const hex = bytes.join('').toUpperCase()
            const mimeType = getMimetype(hex)
            if (!mimeType) {
                alert('Invalid file type');
                event.target.value = '';
                return false;
            }
            if (file.size > chkFileSize) {
                alert('અપલોડ કરેલ દસ્તાવેજ ની સાઇઝ 1 MB કરતાં ઓછી હોવી જોઈએ');
                event.target.value = '';
                return false;
            }
        }


        const getMimetype = (signature) => {
            switch (signature) {
                case '89504E47':
                    return true //'image/png'
                case '25504446':
                    return true //'application/pdf'
                case 'FFD8FFE1':
                    return true //'image/jpeg'
                case 'FFD8FFE0':
                    return true //'image/jpg'
                default:
                    return false // 'Unknown filetype'
            }
        }
    });
}

function ValidateProfilePicture(result) {

    debugger;
    const file = document.getElementById("FormFile123").files[0];
    const filereader = new FileReader()

    filereader.readAsArrayBuffer(file);

    filereader.onloadend = function () {
        debugger;
        var arrayBuffer = filereader.result
        //var bytes = new Uint8Array(arrayBuffer);
        //console.log(bytes);


        //const uint = new Uint8Array(arrayBuffer)
        const uint = (new Uint8Array(arrayBuffer)).subarray(0, 4);
        let bytes = []
        uint.forEach((byte) => {
            bytes.push(byte.toString(16))
        })
        const hex = bytes.join('').toUpperCase()
        const mimeType = getMimetype(hex)
        if (!mimeType) {

            alert('Invalid file type');
            document.getElementById('clock').setAttribute("src", '');
            $("#FormFile123").val('');
            /* $('.custom-file-input').siblings(".custom-file-label").removeClass("selected").html('Choose file');*/
            return false;
        }
        if (file.size > 1000000) {
            alert('Uploaded documents are less than 1 MB');
            document.getElementById('clock').setAttribute("src", '');
            $("#FormFile123").val('');
            /*$('.custom-file-input').siblings(".custom-file-label").removeClass("selected").html('Choose file');*/
            return false;
        }
        else {
            document.getElementById('clock').setAttribute("src", result);
        }
    }


    const getMimetype = (signature) => {
        switch (signature) {
            case '89504E47':
                return true //'image/png'
            case 'FFD8FFE1':
                return true //'image/jpeg'
            case 'FFD8FFE0':
                return true //'image/jpg'
            default:
                return false // 'Unknown filetype'
        }
    }
}
function fileValidation(val) {
    var fileInputsdsdsds = val;
    var fileInput = document.getElementById('file');

    var filePath = fileInput.value;

    // Allowing file type
    var allowedExtensions =
        /(\.jpg|\.jpeg|\.png|\.gif)$/i;

    if (!allowedExtensions.exec(filePath)) {
        alert('Invalid file type');
        fileInput.value = '';
        return false;
    }
    else {

        // Image preview
        if (fileInput.files && fileInput.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                document.getElementById(
                    'imagePreview').innerHTML = '<img src = "' + e.target.result + '" />';
            };

            reader.readAsDataURL(fileInput.files[0]);
        }
    }
}

//function CalculateAge(fromdate, todate, filltext) {
//    debugger;
//    var fromdate = new Date(fromdate);
//    var todate = new Date(todate);

//    var age = todate.getFullYear() - fromdate.getFullYear();
//    $("#" + filltext).val(age);
//}

function CalculateAge(fromdate, todate, filltext) {

    debugger
    var fromdate = new Date(fromdate);
    var dateArray = $('#exdate').val().split('/');
    var date = dateArray[2] + "-" + dateArray[1] + "-" + dateArray[0];
    var todate = new Date(date);

    var age = todate.getFullYear() - fromdate.getFullYear();
    $("#" + filltext).val(age);
}

function AllowNumeric(e) {

    debugger;
    var keyCode = e.which ? e.which : e.keyCode

    if (!((keyCode >= 48 && keyCode <= 57) || keyCode == 46)) {

        return false;
    } else {

    }

}

function AllowAlphaNumeric(e) {

    debugger;
    var k = e.keyCode || e.which;
    var ok = k >= 65 && k <= 90 || // A-Z
        k >= 96 && k <= 105 || // a-z
        k >= 37 && k <= 40 || // arrows
        k == 9 || //tab
        k == 46 || //del
        k == 8 || // backspaces
        (!e.shiftKey && k >= 48 && k <= 57); // only 0-9 (ignore SHIFT options)

    if (!ok || (e.ctrlKey && e.altKey)) {
        e.preventDefault();
    }

}

function CancelOnScheme(serviceId) {
    window.location = "/Home/ApplicationDetails?strserviceId=" + serviceId;
}

$(document).on('change', '.dateValidateCls', function () {
    debugger;
    var date = $(this).val();
    //var date = "19/09/2000";
    if (date != "") {
        var pattern = /(^0[1-9]|[12][0-9]|3[01])\/(0[1-9]|1[0-2])\/(\d{4}$)/;
        var reg = pattern.test(date);
        if (reg == false) {
            //$("#lbldateErrorMsg").css("display", "block");
            //$("#lbldateErrorMsg").text("કૃપા કરીને dd/MM/yyyy ફોર્મેટમાં તારીખ દાખલ કરો.");
            alert("કૃપા કરીને dd/MM/yyyy ફોર્મેટમાં તારીખ દાખલ કરો.");
            $(this).val('');

            return false;
        }
    }
});

//function ValidateDate() {

//    debugger;
//    var date = $("#dob").val();
//    //var date = "19/09/2000";
//    if (date != "") {
//        var pattern = /(^0[1-9]|[12][0-9]|3[01])\/(0[1-9]|1[0-2])\/(\d{4}$)/;
//        var reg = pattern.test(date);
//        if (reg == false) {
//            $("#lbldateErrorMsg").css("display", "block");
//            $("#lbldateErrorMsg").text("કૃપા કરીને dd/MM/yyyy ફોર્મેટમાં તારીખ દાખલ કરો.");
//            return false;
//        }
//        else {
//            $("#lbldateErrorMsg").css("display", "none");
//            $("#lbldateErrorMsg").text("");
//        }
//    }

//}

function getaadharcardcountbyaadharnoandserviceid(aadharcardnoval, serviceid) {
    debugger;
    //var ServiceId = '@serviceid;
    if (serviceid == 2) {
        var aadharcardno = aadharcardnoval[0].value;
    }
    else {
        var aadharcardno = aadharcardnoval.value;

    }
    $.ajax({
        type: "get",
        url: "/Home/Getaadharcardcountbyaadharnoandserviceid",
        data: { aadharcardno: aadharcardno, serviceId: serviceid },
        datatype: "json",
        traditional: true,
        success: function (data) {
            debugger;
            var msg = '@Html.Raw(@ViewBag.Message)';
            if (data.data.applicationcount > 0) {
                swal({
                    title: "",
                    text: data.data.msg,
                    type: "error",
                    confirmButtonClass: "btn-primary",
                });

                $("#aadharcard").val('');

            }
        }
    });
}

