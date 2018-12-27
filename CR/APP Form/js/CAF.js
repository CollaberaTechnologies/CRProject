var divCurrent = '#div1';
var empCount = 1;
var employmentDetails;
var educationDetails;
var eduCount = 1;
var From = 'ibconcierge@collabera.com';
var currentEmployeesArray = new Array();

var requestHeaders = { "accept": "application/json; odata=verbose" };
var DEV_ENVIRONMENT = "DEV";
var PROD_ENVIRONMENT = "PROD";

var ENVIRONMENT = PROD_ENVIRONMENT;
var ServiceURL;
if (ENVIRONMENT == DEV_ENVIRONMENT) {
    ServiceURL = "http://localhost:54958";
} else if (ENVIRONMENT == PROD_ENVIRONMENT) {
    ServiceURL = "https://ibapps.collabera.com/ibwebapi";
}
var svcFileName = 'SPINHelper_IB_WebService.svc';

var globalCountry = '';


//var inputQuantity = [];
//$(function () {
//    $(".yearGraduated").each(function (i) {
//        inputQuantity[i] = this.defaultValue;
//        $(this).data("idx", i); // save this field's index to access later
//    });
//    $(".yearGraduated").on("keyup", function (e) {
//        var $field = $(this),
//            val = this.value,
//            $thisIndex = parseInt($field.data("idx"), 10); // retrieve the index
//        //        window.console && console.log($field.is(":invalid"));
//        //  $field.is(":invalid") is for Safari, it must be the last to not error in IE8
//        if (this.validity && this.validity.badInput || isNaN(val) || $field.is(":invalid")) {
//            this.value = inputQuantity[$thisIndex];
//            return;
//        }
//        if (val.length > Number($field.attr("maxlength"))) {
//            val = val.slice(0, 4);
//            $field.val(val);
//        }
//        inputQuantity[$thisIndex] = val;
//    });
//});

$(document).ready(function () {

    var param1 = new Date();
    var param2 = param1.getDate() + '/' + (param1.getMonth() + 1) + '/' + param1.getFullYear() + ' ' + param1.getHours() + ':' + param1.getMinutes() + ':' + param1.getSeconds();
    $('#currentDateTime').text(param2);
    //$("#dob").kendoDatePicker();
    //$("#fromDate1").kendoDatePicker();
    //$("#toDate1").kendoDatePicker();
    //$("#availabilityBoard").kendoDatePicker({
    //    //value: new Date(),
    //    min: new Date(),
    //    month: {
    //        empty: '<span class="k-state-disabled">#= data.value #</span>'
    //    }
    //});

    // $("#dob").bind("focus", function () {
    //     $(this).data("kendoDatePicker").open();
    // });
    //$("#fromDate1").bind("focus", function () {
    //    $(this).data("kendoDatePicker").open();
    //});
    //$("#toDate1").bind("focus", function () {
    //    $(this).data("kendoDatePicker").open();
    //});
    //$("#availabilityBoard").bind("focus", function () {
    //    $(this).data("kendoDatePicker").open();
    //});
    //$("#availabilityBoard").kendoDatePicker();
	//$("#datepicker").kendoDatePicker();

 //       $("#monthpicker").kendoDatePicker({
 //           // defines the start view
 //           start: "year",

 //           // defines when the calendar should return date
 //           depth: "year",

 //           // display month and year in the input
 //           format: "MMMM yyyy",

 //           // specifies that DateInput is used for masking the input element
 //           dateInput: true
 //       });

    $('#collaberaLogo').click(function () {
        $('#companyLogo').prop('src', "/sites/spin/IB/PublishingImages/India.jpg");
        $('#companyLogo').prop('height', 50);
        $('#companyLogo').prop('style', "float: right;margin-top: -10px !important;margin-right: -15px;");
        $('#entitySelection').hide();
        $('#indiaInfo').show();
        $('.container').show();
        globalCountry = 'India';
        //loadConatctPerson();
        var url = "https://ibapps.collabera.com/ibwebapi/api/CAF/GetContactPerson?LegalEntity=104";
        var options = {
            url: url,
            getValue: "UserName",
            list: {
                maxNumberOfElements: 5,
                match: {
                    enabled: true
                },
                sort: {
                    enabled: false
                }
            },
            theme: "square"
        };
        $(".contactPerson").easyAutocomplete(options);
        GetEmployee(globalCountry);
    });
    $('#nezdaLogo').click(function () {
        $('#companyLogo').prop('src', "/sites/spin/IB/PublishingImages/Philippines.jpg");
        $('#companyLogo').prop('height', 50);
        $('#companyLogo').prop('style', "float: right;margin-top: -10px !important;margin-right: -15px;");
        $('#entitySelection').hide();
        $('#indiaInfo').hide();
        $('.container').show();
        globalCountry = 'Philippines';
        //loadConatctPerson();
        var url = "https://ibapps.collabera.com/ibwebapi/api/CAF/GetContactPerson?LegalEntity=106";
        var options = {
            url: url,
            getValue: "UserName",
            list: {
                maxNumberOfElements: 5,
                match: {
                    enabled: true
                },
                sort: {
                    enabled: false
                }
            },
            theme: "square"
        };
        $(".contactPerson").easyAutocomplete(options);
        GetEmployee(globalCountry);
    });

    $('#dob').change(function (e) {
        debugger;
        var dob = convertDateToYear($('#dob').val());
        var today = convertDateToYear(new Date());
        var parseDob = parseInt(dob);
        var parseToday = parseInt(today);
        var YearDiff = parseToday - parseDob;
        //var age = Math.floor(YearDiff / (365.25 * 24 * 60 * 60 * 1000));
        $('#personal_Age').val(YearDiff);
        $('#age').val(YearDiff);

    });


    $('#presentCompany').click(function () {
        var checked = $('#presentCompany').prop('checked');
        if (checked == true) {
            $('#toDate1').data('kendoDatePicker').enable(false);
            $('#toDate1').val('');
        }
        else {
            $('#toDate1').data('kendoDatePicker').enable(true);
            $('#toDate1').val('');
        }
    });

    //$('#btnSubmit').click(function () {
      
    //    if (validate() == false) {
    //        $('#btnSubmit').attr("disabled", true);
    //        $('#btnBack').attr("disabled", true);
    //        $('#btnClearControls').attr("disabled", true);
    //        //SaveData();
    //    }
    //});

    //$(".delete").click(function () {
    //    debugger;
    //});

    $('#btnBack').click(function () {
        $('#entitySelection').show();
        $('.container').hide();
    });

    $('#btnClearControls').click(function () {
        clearControls();
    });

    $("span.delete:first-child").css("display","none");

    $('.addAditionalRow').click(function () {
        //$($("#employementDetails").first().clone()).appendTo(".addAditionalRow");
        empCount++;
        var getHTMLToappend = '<li class="block-count"><span class="delete" onclick="deleteDiv(this)"><i class="fa fa-times"></i></span><div class="employementDetailsDiv">' +
            '<div class="custpos col-md-6 form-group">' +
            '    <label class="cust-title">Position:</label>' +
            '   <input type="text" name="position" class="position form-control" placeholder="Your Answer" />' +
            '</div>' +
            '<div class="custpos col-md-6 form-group">' +
            '    <label class="cust-title">Company:</label>' +
            '    <input type="text" name="companyName" class="companyName form-control" placeholder="Your Answer" />' +
            '</div>' +
            '<div class="custpos col-md-6 form-group">' +
            '    <label class="cust-title">From Date:</label>' +
            //'    <input type="text" style="width: 100%" class="form-control fromDate" id="fromDate' + empCount + '" placeholder="Your Answer" />' +
            '<div class="input-group date date-picker" data-date-format="dd-mm-yyyy"><input type="text" class="form-control" id="fromDate' + empCount + '"><span class="input-group-btn"><button class="btn default" type="button"><i class="fa fa-calendar"></i></button></span></div>' +
            '</div>' +
            '<div class="custpos col-md-6 form-group">' +
            '    <label class="cust-title">To Date: </label>' +
            //'    <input type="text" style="width: 100%" class="form-control toDate" id="toDate' + empCount + '" placeholder="Your Answer" />' +
            '<div class="input-group date date-picker" data-date-format="dd-mm-yyyy"><input type="text" class="form-control" id="toDate' + empCount + '"><span class="input-group-btn"><button class="btn default" type="button"><i class="fa fa-calendar"></i></button></span></div>' +
            '</div>' +
            '<div class="custpos col-md-12 form-group">' +
            '    <label class="cust-title">Skills/Technology Used:</label>' +
            '    <input type="text" class="skillsAndTechnology form-control" placeholder="Your Answer">' +
            '</div></div></li>';

        $("#employementDetails").append(getHTMLToappend);
        //$("#fromDate" + empCount).kendoDatePicker();
        //$("#toDate" + empCount).kendoDatePicker();
        //$("#fromDate" + empCount).bind("focus", function () {
        //    $(this).data("kendoDatePicker").open();
        //});
        //$("#toDate" + empCount).bind("focus", function () {
        //    $(this).data("kendoDatePicker").open();
        //});
        $(".date-picker").datepicker({
            orientation: "right",
            autoclose: !0,
            format: 'mm/dd/yyyy'
        });
        
        //$("#toDate" + empCount).change(function () {
        //    if ($("#fromDate" + empCount).val() == '') {
        //        swal({ title: "Required", text: "Please select Start Date", type: "error" });
        //        $("#toDate" + empCount).val('');
        //    } else {
        //        var CurrentValue = $(this).val();
        //        if (CurrentValue <= $("#fromDate" + empCount).val()) {
        //            swal({ title: "Required", text: "End Date should be greater than Start Date", type: "error" });
        //            $("#toDate" + empCount).val('');
        //        }
        //    }
        //});
    });

    $('.addAditionaleduRow').click(function () {
        //$($("#employementDetails").first().clone()).appendTo(".addAditionalRow");
        eduCount++;
        var getHTMLToappend = '<li class="block-count"><span class="delete" onclick="deleteDiv(this)"><i class="fa fa-times"></i></span><div class="row educationDetailsDiv">' +
            '<div class="custpos col-md-6 form-group">' +
            '    <label class="cust-title">Course: <span class="reqfield">*</span></label>' +
            '   <input type="text" name="Course" class="course form-control" placeholder="Your Answer" />' +
            '</div>' +
            '<div class="custpos col-md-6 form-group">' +
            '    <label class="cust-title">Year Graduated: <span class="reqfield">*</span></label>' +
            '    <input type="number" maxlength="4" name="yearGraduated" class="yearGraduated form-control" placeholder="Your Answer" />' +
            '</div>' +
            '<div class="custpos col-md-6 form-group">' +
            '    <label class="cust-title">School/University: <span class="reqfield">*</span></label>' +
            '    <input type="text" name="university" class="university form-control" placeholder="Your Answer" />' +
            '</div>' +
            '<div class="custpos col-md-6 form-group">' +
            '    <label class="cust-title">Other Trainings Attended:</label>' +
            '    <input type="text" name="otherTrainingsAttended" class="otherTrainingsAttended form-control" placeholder="Your Answer" />' +
            '</div>' +
            '</div></li>';

        $("#educationDetails").append(getHTMLToappend);
    });

    //$("#toDate1").change(function () {
    //    if ($("#fromDate1").val() == '') {
    //        swal({ title: "Required", text: "Please select Start Date", type: "error" });
    //        $("#toDate1").val('');
    //    } else {
    //        var CurrentValue = $(this).val();
    //        if (CurrentValue <= $("#fromDate1").val()) {
    //            swal({ title: "Required", text: "End Date should be greater than Start Date", type: "error" });
    //            $("#toDate1").val('');
    //        }
    //    }
    //});
    
});

function convertDateToYear(dateObject) {
    var d = new Date(dateObject);
    var month = d.getMonth() + 1;
    var day = d.getDate();
    var output =
        d.getFullYear() + '/' +
        (('' + month).length < 2 ? '0' : '') + month + '/' +
        (('' + day).length < 2 ? '0' : '') + day;

    return output;
}

function clearControls() {
    $('input').val('');
    $('input:radio').prop("checked", false);
    $('#educationDetails li:not(:first)').remove();
    $('#employementDetails li:not(:first)').remove();
}

function GetEmployee(country) {
    var uri = '';
    if (country == 'India') {
        uri = "https://ibapps.collabera.com/ibwebapi/api/CAF/GetContactPerson?LegalEntity=104"
    } else {
        uri = "https://ibapps.collabera.com/ibwebapi/api/CAF/GetContactPerson?LegalEntity=106"
    }
    $("#contactPersonDiv").html('');
    $('#contactPersonDiv').append('<label class="cust-title">Contact Person:<span class= "reqfield">*</span></label><input name="contactPerson" id="contactPerson" class="contactPerson form-control" placeholder="Your Answer">');
    //$("#contactPerson").kendoComboBox({
    //    placeholder:'--- Select Contact Person ---',
    //    dataTextField: "UserName",
    //    dataValueField: "ID",
    //    filter:"contains",        
    //    suggest: true,
    //    dataSource:{
    //        type:"json",
    //        serverFiltering:false,
    //        transport:{
    //            read:{
    //                url:uri,
    //                beforeSend: function (xhr) {
    //                    xhr.setRequestHeader("Accept", "application/json; odata=verbose")
    //                }
    //            }
    //        }
    //    },
    //    schema: {
    //        data: function (data) {
    //            return data.d && data.d.results ? data.d.results : [data.d];
    //        }
    //    }
    //});  
    //GetEmployeeForValidate(country);
}


//function GetEmployeeForValidate(country) {
//    var uri = '';
//    if (country == 'India') {
//        uri = "https://ibapps.collabera.com/ibwebapi/api/CAF/GetContactPerson?LegalEntity=104"
//    } else {
//        uri = "https://ibapps.collabera.com/ibwebapi/api/CAF/GetContactPerson?LegalEntity=106"
//    }
//    $.ajax({
//        url: uri,
//        type: "GET",
//        dataType: "json",
//        headers: requestHeaders,
//        async: true,
//        success: function (data) {
//            if (data) {
//                currentEmployeesArray = new Array();
//                for (i = 0; i < data.length; i++) {
//                    currentEmployeesArray.push({ ID: data[i].ID, Email: data[i].UserName });
//                }
//            }
//        },
//        error: function (error) {
//            console.log(error)
//        }
//    })
//}

function deleteDiv(control) {
    $(control).parent().remove();
}

// function loadConatctPerson(){
//     var url ="https://ibapps.collabera.com/ibwebapi/api/CAF/GetContactPerson?LegalEntity="+globalCountry;
//     var options = {
//         url: url,
//         getValue : "User_ID",
//         list: {
//             maxNumberOfElements: 5,
//             match: {
//                 enabled: true
//             },
//             sort: {
//                 enabled: false
//             }
//         },    
//         theme: "square"
//     };    
// }

function validate() {
    debugger;
    var isValidated = true;
    var msg = '';
    
    if ($('#contactPerson').val() == '') {
        isValidated = false;
        msg += 'Contact Person is required \n';
    } else {
        //var found = currentEmployeesArray.some(function (el) {
        //    return el.Email === $('#contactPerson').val();
        //});
        //if (!found) {
        //    isValidated = false;
        //    msg += 'Enter proper Contact Person Name \n';
        //}
    }
    
    if ($('#candidateName').val() == '') {
        isValidated = false;
        msg += 'Name is required \n';
    }
    if ($('#txtlanguage').val() == '') {
        isValidated = false;
        msg += 'Language Known is required \n';
    }
   
    if ($('#txtKnow').val() == '') {
        isValidated = false;
        msg += 'What do you Known Collabera is required \n';
    }

    if ($('#txtaspiration').val() == '') {
        isValidated = false;
        msg += 'What are your Career aspirations is required \n';
    }
    if ($('#EmailId').val() == '') {
        isValidated = false;
        msg += 'EmailId is required \n';
    }
    else if ((/^((\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)\s*[;]{0,1}\s*)+$/.test($('#EmailId').val()))) { 
    }
    else {
        isValidated = false;
        msg += "Please Enter correct Email-Id\n";

    }
    if ($('#CurrentCTC').val() == '') {
        isValidated = false;
        msg += 'CurrentCTC is required \n';
    }
   if ($('#Placetomeet').val() == '') {
        isValidated = false;
        msg += 'Placetomeet is required \n';
    }
    if ($('#positionDesired').val() == '') {
        isValidated = false;
        msg += 'Position Desired is required \n';
    }
    if ($('#expectedSalary').val() == '') {
        isValidated = false;
        msg += 'Expected Salary is required \n';
    }
    if ($('#availabilityBoard').val() == '') {
        isValidated = false;
        msg += 'Availability on board is required \n';
    } else if (new Date($('#availabilityBoard').val()) < new Date()) {
        isValidated = false;
        msg += "Availability on board date should be greater than today's Date \n";
    } else {
       
    }
    if ($('#Datetimetomeet').val() == '') {
        isValidated = false;
        msg += 'Date time to meet is required \n';
    } else if (new Date($('#Datetimetomeet').val()) < new Date()) {
        isValidated = false;
        msg += "Date time to meet  should be greater than today's Date \n";
    } else {
       
    }
   

    if ($('#candidateAddress').val() == '') {
        isValidated = false;
        msg += 'Address is required \n';
    }
    if ($('#mobileNumber').val() == '') {
        isValidated = false;
        msg += 'Mobile Number is required \n';
    }
    //if ($('input[name=personal.Gender]:checked').length <= 0) {
    //    isValidated = false;
    //    msg += 'Gender is required \n';
    //}
    if ($('#dob').val() == '') {
        isValidated = false;
        msg += 'Date of Birth is required \n';
    } else {
        
    }

    if ($('#age').val() == '') {
        isValidated = false;
        msg += 'Age is required \n';
    }
    if ($('#maritalStatus').val() == '') {
        isValidated = false;
        msg += 'Marital Status is required \n';
    }

    $('.educationDetailsDiv').each(function () {
        if ($(this).find('.course').val() == '') {
            isValidated = false;
            msg += 'Course is required \n';
        }
        if ($(this).find('.yearGraduated').val() == '') {
            isValidated = false;
            msg += 'Year Graduated is required \n';
        } else if ($(this).find('.yearGraduated').val().length > 4 || $(this).find('.yearGraduated').val().length < 4) {
            isValidated = false;
            msg += 'Year Graduated is invalid \n';
        }
        if ($(this).find('.university').val() == '') {
            isValidated = false;
            msg += 'University is required \n';
        }
        if ($(this).find('.otherTrainingsAttended').val() == '') {
            isValidated = false;
            msg += 'otherTrainingsAttended is required \n';
        }

    });

    if ($('#referenceName').val() == '') {
        isValidated = false;
        msg += 'Please enter Reference Name \n';
    }

    if ($('#referenceRelationship').val() == '') {
        isValidated = false;
        msg += 'Please enter Reference Relationship \n';
    }

    if ($('input[name=declaration]:checked').length <= 0) {
        isValidated = false;
        msg += 'Please select the Declaration \n';
    }
    if (!isValidated) {
        swal({
            title: "Required",
            text: msg,
            type: "error"
        });
    }
    return isValidated;
}
//('input[name=declaration]').change(function () {
//    debugger;
//    $('#btnSubmit').attr("disabled", false);

//});
$('#EmailId').change(function () {
    debugger;
    if ((/^((\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)\s*[;]{0,1}\s*)+$/.test($('#EmailId').val()))) {
    }
    else {
        swal({
            title: "Required",
            text: "Please Enter Correct Email-Id.",
            type: "error"
        });

    }
})
function isDate(ExpiryDate) {
    var objDate,  // date object initialized from the ExpiryDate string 
        mSeconds, // ExpiryDate in milliseconds 
        day,      // day 
        month,    // month 
        year;     // year 
    // date length should be 10 characters (no more no less) 
    if (ExpiryDate.length !== 10) {
        return false;
    }
    // third and sixth character should be '/' 
    if (ExpiryDate.substring(2, 3) !== '/' || ExpiryDate.substring(5, 6) !== '/') {
        return false;
    }
    // extract month, day and year from the ExpiryDate (expected format is mm/dd/yyyy) 
    // subtraction will cast variables to integer implicitly (needed 
    // for !== comparing) 
    month = ExpiryDate.substring(0, 2) - 1; // because months in JS start from 0 
    day = ExpiryDate.substring(3, 5) - 0;
    year = ExpiryDate.substring(6, 10) - 0;
    // test year range 
    if (year < 1000 || year > 3000) {
        return false;
    }
    // convert ExpiryDate to milliseconds 
    mSeconds = (new Date(year, month, day)).getTime();
    // initialize Date() object from calculated milliseconds 
    objDate = new Date();
    objDate.setTime(mSeconds);
    // compare input date and parts from Date() object 
    // if difference exists then date isn't valid 
    if (objDate.getFullYear() !== year ||
        objDate.getMonth() !== month ||
        objDate.getDate() !== day) {
        return false;
    }
    // otherwise return true 
    return true;
}


function SaveData() {
    var contactPerson = $('#contactPerson').val();
    var positionDesired = $('#positionDesired').val();
    var expectedSalary = $('#expectedSalary').val();
    var workLocation = $('#workLocation').val();
    var availabilityBoard = $('#availabilityBoard').val();
    var candidateName = $('#candidateName').val();
    var candidateAddress = $('#candidateAddress').val();
    var mobileNumber = $('#mobileNumber').val();
    var contactPhone = $('#contactPhone').val();
    var gender = $('input[name=gender]:checked').val();
    var dob = $('#dob').val();
    var age = $('#age').val().trim();
    var maritalStatus = $('#maritalStatus option:selected').val();
    var course = $('#course').val();
    var yearGraduated = $('#yearGraduated').val();
    var university = $('#university').val();
    var otherTrainingsAttended = $('#otherTrainingsAttended').val();
    var position = $('.position').val();
    var companyName = $('.companyName').val();
    var LanguagesKnown = $('#txtlanguage').val();
    //var HowKnowAboutCollabera = $('input[name=optradio]:checked').val();
    var HowKnowAboutCollabera = '';
    if ($('input[name=optradio]:checked').val() == 'Collabera Connected You') {
        HowKnowAboutCollabera = $('#CollaberaConnectedYou').val();
    } else if ($('input[name=optradio]:checked').val() == 'Someone referred You to Collabera') {
        HowKnowAboutCollabera = $('#SomeoneReferredYou').val();
    } else {
        HowKnowAboutCollabera = $('input[name=optradio]:checked').val();
    }
    var WhatKnownAboutCollabera = $('#txtKnow').val();
    var Careeraspirations = $('#txtaspiration').val();
    var strengthsandweakness = $('#txtStrengthAndWeakness').val();


    var fromDate = $('#fromDate1').val();
    var skillsAndTechnology = $('.skillsAndTechnology').val();

    var referenceName = $('#referenceName').val();
    var referenceRelationship = $('#referenceRelationship').val();
    var referenceNumber = $('#referenceNumber').val().trim();
    var referenceNameOther = $('#referenceNameOther').val();
    var referenceRelationshipOther = $('#referenceRelationshipOther').val();
    var referenceNumberOther = $('#referenceNumberOther').val().trim();
    var referenceNameExtra = $('#referenceNameExtra').val();
    var referenceRelationshipExtra = $('#referenceRelationshipExtra').val();
    var referenceNumberExtra = $('#referenceNumberExtra').val().trim();

    InsertData = {
        "contactperson": contactPerson,
        "expsalary": parseInt(expectedSalary),
        "Availonboard": availabilityBoard,
        "Posdesire": positionDesired,
        "Preferloc": workLocation,
        "name": candidateName,
        "Address": candidateAddress,
        "Mobile": mobileNumber,
        "Phone_": contactPhone,
        "Gender": gender,
        "Dob": dob,
        "Age": parseInt(age),
        "Maritalstatus": maritalStatus,
        "FLAG": true,
        //"Created_Date": "2018-05-23T08:35:04.706Z",
        //"Modified_Date": "2018-05-23T08:35:04.706Z",
        //"Created_User": "string",
        //"Modified_User": "string",
        "Country": globalCountry,
        "LanguagesKnown": LanguagesKnown,
        "HowKnowAboutCollabera": HowKnowAboutCollabera,
        "WhatKnownAboutCollabera": WhatKnownAboutCollabera,
        "Careeraspirations": Careeraspirations,
        "strengthsandweakness": strengthsandweakness
    }



    var rowCount = 0;
    employmentDetails = new Array();
    $(".employementDetailsDiv").each(function () {
        rowCount++;
        //var toDate = $('#toDate' + rowCount).val();
        var toDate = $(this).find("input.toDate").val();
        if (rowCount == 1) {
            var checked = $('#presentCompany').prop('checked');
            if (checked == true) {
                toDate = null;
            }
        }
        var empFromDate = $(this).find("input.fromDate").val();
        var empToDate = toDate == null ? '' : toDate;
        var position = $(this).find("input.position").val();
        var companyName = $(this).find("input.companyName").val();
        var skillsAndTechnology = $(this).find("input.skillsAndTechnology").val();
        if ($.trim(empFromDate) != '' || $.trim(empToDate) != '' || $.trim(position) != '' || $.trim(companyName) != '' || $.trim(skillsAndTechnology) != '') {
            employmentDetails.push({ "position": position, "fromdate": empFromDate, "todate": empToDate, "skills": skillsAndTechnology, "company": companyName });
        }
    });
    InsertData.lstRecent_Employeer = employmentDetails;

    var rowEduCount = 0;
    educationDetails = new Array();
    $(".educationDetailsDiv").each(function () {
        rowEduCount++;
        var course = $(this).find("input.course").val() == '' ? '' : $(this).find("input.course").val();
        var yearGraduated = $(this).find("input.yearGraduated").val() == '' ? "" : $(this).find("input.yearGraduated").val().toString();
        var university = $(this).find("input.university").val() == '' ? '' : $(this).find("input.university").val();
        var otherTrainingsAttended = $(this).find("input.otherTrainingsAttended").val();
        educationDetails.push({ "course": course, "Yrgraduated": yearGraduated, "SchoolUniversity": university, "Othertrainings": otherTrainingsAttended });
    });
    InsertData.lstEdu_Background = educationDetails;
    var RefernceArray = new Array();
    if ($('#referenceName').val() != '' && $('#referenceRelationship').val() != '') {
        RefernceArray.push({ "name": $('#referenceName').val(), "relationship": $('#referenceRelationship').val(), "contactno": $('#referenceNumber').val() ? $('#referenceNumber').val() : "" });
    }
    if ($('#referenceNameOther').val() != '' && $('#referenceRelationshipOther').val() != '') {
        RefernceArray.push({ "name": $('#referenceNameOther').val(), "relationship": $('#referenceRelationshipOther').val(), "contactno": $('#referenceNumberOther').val() ? $('#referenceNumberOther').val() : "" });
    }
    if ($('#referenceNameExtra').val() != '' && $('#referenceRelationshipExtra').val() != '') {
        RefernceArray.push({ "name": $('#referenceNameExtra').val(), "relationship": $('#referenceRelationshipExtra').val(), "contactno": $('#referenceNumberExtra').val() ? $('#referenceNumberExtra').val() : "" });
    }

    //RefernceArray.push(
    //    { "name": $('#referenceName').val(), "relationship": $('#referenceRelationship').val(), "contactno": $('#referenceNumber').val() ? $('#referenceNumber').val() : "" },
    //    { "name": $('#referenceNameOther').val(), "relationship": $('#referenceRelationshipOther').val(), "contactno": $('#referenceNumberOther').val() ? $('#referenceNumberOther').val() : "" },
    //    { "name": $('#referenceNameExtra').val(), "relationship": $('#referenceRelationshipExtra').val(), "contactno": $('#referenceNumberExtra').val() ? $('#referenceNumberExtra').val() : "" }
    //);
    InsertData.lstReference = RefernceArray;

    $.ajax({
        url: "https://ibapps.collabera.com/ibwebapi/api/CAF/CreateCAF",
        type: "POST",
        data: JSON.stringify(InsertData),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (resultData) {
            if (resultData) {
                createDataForPDF();
            } else {
                alert('Error');
            }

        },
        failure: function (response) {
            console.log("Faliure");
        },
        error: function (Error) {
            console.log("Error");
        }
    });
}
function createDataForPDF() {
    $.get("https://collaberainc.sharepoint.com/sites/spin/IB/hr/HTML%20Pages/resumePrintCAF.html", function (data) {
        $("#ResumePDF").html(data);
       
        $('#PDFName').text($('#candidateName').val());
        $('#PDFGender').text($('input[name=gender]:checked').val());
        $('#PDFDOB').text($('#dob').val());
        $('#PDFAge').text($('#age').val());
        $('#PDFAddress').text($('#candidateAddress').val());
        $('#PDFMobile').text($('#mobileNumber').val());
        $('#PDFPhone').text($('#contactPhone').val());
        $('#PDFMaritalStatus').text($('#maritalStatus option:selected').val());
        var rowCount = 0;
        $(".employementDetailsDiv").each(function () {
            rowCount++;
            var toDate = $('#toDate' + rowCount).val();
            var empFromDate = $('#fromDate' + rowCount).val();
            var empToDate = toDate;
            var position = $(this).find("input.position").val();
            var companyName = $(this).find("input.companyName").val();
            var skillsAndTechnology = $(this).find("input.skillsAndTechnology").val();
            $('#employmentTablePDF tbody').last().append('<tr><td>' + rowCount + '</td><td>' + position + '</td><td>' + companyName + '</td><td>' + empFromDate + '</td><td>' + empToDate + '</td><td>' + skillsAndTechnology + '</td></tr>');
        });

        if ($('#employmentTablePDF tbody').html != "") {
            $('#RecentEmployerPDFDiv').hide();
        } else {
            $('#RecentEmployerPDFDiv').show();
        }

        var rowEduCount = 0;
        $(".educationDetailsDiv").each(function () {
            rowEduCount++;
            var course = $(this).find("input.course").val();
            var yearGraduated = $(this).find("input.yearGraduated").val();
            var university = $(this).find("input.university").val();
            var otherTrainingsAttended = $(this).find("input.otherTrainingsAttended").val();
            $('#educationTablePDF tbody').last().append('<tr><td>' + rowEduCount + '</td><td>' + course + '</td><td>' + yearGraduated + '</td><td>' + university + '</td><td>' + otherTrainingsAttended + '</td></tr>');
        });
        if ($('#referenceName').val() != '' || $('#referenceRelationship').val() != '' || $('#referenceNumber').val() != '') {
            $('#refernceTablePDF tbody').last().append('<tr><td>' + $('#referenceName').val() + '</td><td>' + $('#referenceRelationship').val() + '</td><td>' + $('#referenceNumber').val() + '</td></tr>');
        }
        if ($('#referenceNameOther').val() != '' || $('#referenceRelationshipOther').val() != '' || $('#referenceNumberOther').val() != '') {
            $('#refernceTablePDF tbody').last().append('<tr><td>' + $('#referenceNameOther').val() + '</td><td>' + $('#referenceRelationshipOther').val() + '</td><td>' + $('#referenceNumberOther').val() + '</td></tr>');
        }
        if ($('#referenceNameExtra').val() != '' || $('#referenceRelationshipExtra').val() != '' || $('#referenceNumberExtra').val() != '') {
            $('#refernceTablePDF tbody').last().append('<tr><td>' + $('#referenceNameExtra').val() + '</td><td>' + $('#referenceRelationshipExtra').val() + '</td><td>' + $('#referenceNumberExtra').val() + '</td></tr>');
        }

        var HowDidYouKnowAboutCollabera = '';
        if ($('input[name=optradio]:checked').val() == 'Collabera Connected You') {
            HowDidYouKnowAboutCollabera = $('#CollaberaConnectedYou').val();
        } else if ($('input[name=optradio]:checked').val() == 'Someone referred You to Collabera') {
            HowDidYouKnowAboutCollabera = $('#SomeoneReferredYou').val();
        } else {
            HowDidYouKnowAboutCollabera = $('input[name=optradio]:checked').val();
        }

        if (globalCountry == 'India') {
            $('#officePDF').text('CH1');
            //$('#indiaInfoPrint').show();
            $('#additionalTablePDF tbody').append('<tr><td>' + $('#txtlanguage').val() + '</td><td>' + (HowDidYouKnowAboutCollabera == undefined ? '' : undefined) + '</td><td>' + $('#txtKnow').val() + '</td><td>' + $('#txtaspiration').val() + '</td><td>' + $('#txtStrengthAndWeakness').val() + '</td></tr>')
        } else {
            $('#officePDF').text('PH');
            $('#indiaInfoPrint').hide();
        }
        
        $('#contactPDF').text($('#contactPerson').val());
        $('#contactPersonPDF').text($('#contactPerson').val());
        $('#positionDesiredPDF').text($('#positionDesired').val());
        $('#availOnBoardPDF').text($('#availabilityBoard').val());
        $('#ExpectedSalaryPDF').text($('#expectedSalary').val());
        $('#preferredLocationPDF').text($('#workLocation').val());
        //demoFromHTML();
        HTMLToAppend = $("#ResumePDF").html();
        sendEmail();
    });
    
}

function demoFromHTML() {
    var doc = new jsPDF();
    var elementHandler = {
        '#ResumePDF': function (element, renderer) {
            return true;
        }
    };
    var source = window.document.getElementsByClassName("pcontainer")[0];
    doc.fromHTML(
        source,
        15,
        15,
        {
            'width': 180, 'elementHandlers': elementHandler
        });
    sendEmail(doc.output()); 
}
var HTMLToAppend = '';
//HTMLToAppend += "<script>"+
//"function CallPrint(strid)" +
//"{" +
//"var prtContent = document.getElementById(strid)" +
//"var WinPrint = window.open('', '', 'left=0,top=0,width=700,height=500,toolbar=0,scrollbars=0,status=0')" +
//"WinPrint.document.write(prtContent.innerHTML)" +
//"WinPrint.document.close()" +
//"WinPrint.focus()" +
//"WinPrint.print()" +
//"WinPrint.close()" +
//"prtContent.innerHTML = strOldOne" +
//"}" +
//"</script>" +
//"<div id='bholder'>Content to be printed</div>" +
//"<input type='button' value='Print' onClick='javascript:CallPrint('bholder')'/>";

function sendEmail(attachmentContent) {
    var toName = '';
    if ($('#contactPerson').val() == 'Front Office') {
        toName = 'sandhya.adesai@collabera.com';
    } else {
        toName = $('#contactPerson').val() + '@collabera.com';
    }
    
    var cc = new Array();
    cc.push("isha.kapoor@collabera.com");
    cc.push("sakil.vhora@collabera.com");
    cc.push("sandhya.adesai@collabera.com");
    cc.push("trideep.joshi@collabera.com");
    cc.push("sitakanta.shial@collabera.com");
    var constructedURL = _spPageContextInfo.webServerRelativeUrl + "/_api/SP.Utilities.Utility.SendEmail";
    $.ajax({
        contentType: 'application/json',
        url: constructedURL,
        type: "POST",
        data: JSON.stringify({
            'properties': {
                '__metadata': { 'type': 'SP.Utilities.EmailProperties' },
                'From': From,
                'To': { 'results': [toName] },
                'CC': { 'results': cc },
                'Body': HTMLToAppend,
                'Subject': 'Candidate Application Form - ' + $('#candidateName').val(),
                'AdditionalHeaders' : {
                    "__metadata": {
                        "type": "Collection(SP.KeyValue)"
                    },
                    "results": [{
                        "__metadata": {
                            "type": "SP.KeyValue"
                        },
                        "Key": "content-type",
                        "Value": "text/html",
                        "ValueType": "Edm.String"
                    }]
                }
            }
        }
        ),
        headers: {
            "Accept": "application/json;odata=verbose",
            "content-type": "application/json;odata=verbose",
            "X-RequestDigest": $("#__REQUESTDIGEST").val()
        },
        success: function (data) {
            //alert('Email Sent Successfully');
            swal({
                title: "Success",
                text: "Collabera will use the data provided by you for the purpose of current/future legitimate recruitment process.",
                type: "success"
            });
            setTimeout(function () {
                window.location.replace("https://collaberainc.sharepoint.com/sites/spin/IB/hr/SitePages/CAF.aspx");
            }, 3000);
            //swal({
            //    title: "success!",
            //    text: "Collabera will use the data provided by you for the purpose of current/future legitimate recruitment process.",
            //    type: "success"
            //}).then(okay => {
            //    if (okay) {
            //        window.location.replace("https://collaberainc.sharepoint.com/sites/spin/IB/hr/SitePages/CAF.aspx");    
            //    }
            //});
        },
        error: function (err) {
            alert('Error in sending Email: ' + JSON.stringify(err));
        }
    });
}




