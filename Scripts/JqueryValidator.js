//Function to allow only numbers to textbox OnlyNumber
function OnlyNumber(evt, id) {
    //getting key code of pressed key
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (document.getElementById(id).value === '') {
        if (evt.which === 32)
            return false;
    }
    if (evt.which === 13) {
        return false;
    }
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}

function OnlyNumberAftertwoDigits(evt, id) {
    //getting key code of pressed key
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (document.getElementById(id).value === '') {
        if (evt.which === 32)
            return false;
    }
    if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode !== 46)
        return false;

    if (evt.which === 13)
        return false;

    // apply the two-digits behaviour to elements with 'two-digits' as their class
    Number.prototype.toFixedDown = function (digits) {
        var n = this - Math.pow(10, -digits) / 2;
        n += n / Math.pow(2, 53); // added 1360765523: 17.56.toFixedDown(2) === "17.56"
        return n.toFixed(digits);
    }

    $(function () {
        $('#txtContainerPrice').keyup(function () {
            if ($(this).val().indexOf('.') !== -1) {
                if ($(this).val().split(".")[1].length > 2) {
                    if (isNaN(parseFloat(this.value))) return;
                    this.value = parseFloat(this.value).toFixedDown(2);
                }
            }
            return this; //for chaining
        });
    });

    $(function () {
        $('#txtUnitPrice').keyup(function () {
            if ($(this).val().indexOf('.') !== -1) {
                if ($(this).val().split(".")[1].length > 2) {
                    if (isNaN(parseFloat(this.value))) return;
                    this.value = parseFloat(this.value).toFixedDown(2);
                }
            }
            return this; //for chaining
        });
    });

    $(function () {
        $('#txtLedgerTransactAmt').keyup(function () {
            if ($(this).val().indexOf('.') !== -1) {
                if ($(this).val().split(".")[1].length > 2) {
                    if (isNaN(parseFloat(this.value))) return;
                    this.value = parseFloat(this.value).toFixedDown(2);
                }
            }
            return this; //for chaining
        });
    });

    $(function () {
        $('#txtContainerPrice').keyup(function () {
            if ($(this).val().indexOf('.') !== -1) {
                if ($(this).val().split(".")[1].length > 2) {
                    if (isNaN(parseFloat(this.value))) return;
                    this.value = parseFloat(this.value).toFixedDown(2);
                }
            }
            return this; //for chaining
        });
    });

    $(function () {
        $('#txtPayrollAmount').keyup(function () {
            if ($(this).val().indexOf('.') !== -1) {
                if ($(this).val().split(".")[1].length > 2) {
                    if (isNaN(parseFloat(this.value))) return;
                    this.value = parseFloat(this.value).toFixedDown(2);
                }
            }
            return this; //for chaining
        });
    });

    $(function () {
        $('#txtOtherExpenseAmt').keyup(function () {
            if ($(this).val().indexOf('.') !== -1) {
                if ($(this).val().split(".")[1].length > 2) {
                    if (isNaN(parseFloat(this.value))) return;
                    this.value = parseFloat(this.value).toFixedDown(2);
                }
            }
            return this; //for chaining
        });
    });

    $(function () {
        $('#txtMaximum').keyup(function () {
            if ($(this).val().indexOf('.') !== -1) {
                if ($(this).val().split(".")[1].length > 2) {
                    if (isNaN(parseFloat(this.value))) return;
                    this.value = parseFloat(this.value).toFixedDown(2);
                }
            }
            return this; //for chaining
        });
    });

    return true;
}


//Function to allow only numbers and Dot to textbox
function NumberandDot(evt, id) {
    //getting key code of pressed key
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (document.getElementById(id).value === '') {
        if (evt.which === 32)
            return false;
    }

    if (evt.which === 13) {
        return false;
    }

    var val1 = $(document.getElementById(id)).val();

    if (isNaN(val1)) {

        //val1 = val1.replace(/[^0-9\.]/g, '');
        //if (val1.split('.').length > 1)
        //    val1 = val1.replace(/\.+$/, "");
    } 


    $(document.getElementById(id)).val(val1);

    if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode !== 46 && charCode !== 13)
        return false;
    return true;

}



function Address(evt, id) {
    //getting key code of pressed key
    var charCode = (evt.which) ? evt.which : event.keyCode;
    //if (document.getElementById(id).value === '') {
    //    if (evt.which === 32)
    //        return false;
    //}
    if (charCode === 62 || charCode === 60 || charCode === 94)
        return false;
    return true;
}


function validate(evt, id) {
    var charCode = (evt.which) ? evt.which : event.keyCode;

    if (document.getElementById(id).value === '') {
        if (evt.which === 32)
            return false;
    }
    if (evt.which === 13) {
        return false;
    }
    // if (charCode !== 32 && charCode > 31 && (charCode < 48 || charCode > 57))
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}

//Function to allow ValiDating Alpha Numeric
function AlphaNumeric(evt, id) {
    var charCode = (evt.which) ? evt.which : event.keyCode;

    if (document.getElementById(id).value === '') {
        if (evt.which === 32)
            return false;
    }
    if (evt.which === 13) {
        return false;
    }
    // if (charCode !== 32 && charCode > 31 && (charCode < 48 || charCode > 57))
    if (charCode > 31 && (charCode < 48 || charCode > 57) && (charCode < 97 || charCode > 122) && (charCode < 65 || charCode > 90) && charCode !== 45 && charCode !== 32)
        return false;
    return true;
}

//Function to allow ValiDating Alpha Numeric
function AlphaNumericExceptEnter(evt, id) {
    var charCode = (evt.which) ? evt.which : event.keyCode;

    if (document.getElementById(id).value === '') {
        if (evt.which === 32)
            return false;
    }
    // if (charCode !== 32 && charCode > 31 && (charCode < 48 || charCode > 57))
    if (charCode > 31 && (charCode < 48 || charCode > 57) && (charCode < 97 || charCode > 122) && (charCode < 65 || charCode > 90) && charCode !== 45 && charCode !== 32)
        return false;
    return true;
}



function validatePhone(evt, id) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (document.getElementById(id).value === '') {
        if (evt.which === 32)
            return false;
    }
    if (evt.which === 13) {
        return false;
    }
    var textBox = document.getElementById(id);
    var textLength = textBox.value.length;
    if (textLength < 1) {

        if (evt.which === 43)
            return true;
    }
    // if (charCode !== 32 && charCode > 31 && (charCode < 48 || charCode > 57))
    if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode !== 45 && charCode !== 47)
        return false;
    return true;
}


//Function to allow ValiDating Fax Number
function validateFax(key, id) {
    //getting key code of pressed key
    var charCode = (key.which) ? key.which : event.keyCode;
    var textBox = document.getElementById(id);
    var textLength = textBox.value.length;
    if (textLength < 1) {

        if (key.which === 43)
            return true;
    }
    if (key.which === 13) {
        return false;
    }
    //comparing pressed keycodes
    //if (!(keycode === 8 || keycode === 9 || keycode === 45 || keycode === 41 || keycode === 40 || keycode === 43 || keycode === 32 || keycode === 37 || keycode === 38 || keycode === 39 || keycode === 40) && (keycode < 48 || keycode > 57)) {
    if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode !== 45) {
        return false;
    }

}

//Function to allow ValiDating Charter Only
function CharterOnly(key, id) {
    //getting key code of pressed key

    var charCode = (key.which) ? key.which : event.keyCode;
    if (document.getElementById(id).value === '') {
        if (key.which === 32)
            return false;
    }
    if (key.which === 13)
        return false;
    //comparing pressed keycodes
    if (charCode > 31 && (charCode < 97 || charCode > 122) && (charCode < 65 || charCode > 90) && charCode !== 32) {
        return false;

        //if (!(keycode === 8 || keycode === 9 || keycode === 32 || keycode === 46 || keycode === 37 || keycode === 38 || keycode === 39 || keycode === 40) && (keycode < 97 || keycode > 122) && (keycode < 65 || keycode > 90)) {
        //    return false;
    }


}
//Function to allow ValiDating Company Name
function validateCompany(evt, id) {
    var charCode = (evt.which) ? evt.which : event.keyCode;

    if (document.getElementById(id).value === '') {
        if (evt.which === 32)
            return false;
    }
    if (evt.which === 13) {
        return false;
    }
    // if (charCode !== 32 && charCode > 31 && (charCode < 48 || charCode > 57))
    if (charCode > 31 && (charCode < 48 || charCode > 57) && (charCode < 97 || charCode > 122) && (charCode < 65 || charCode > 90) && charCode !== 45 && charCode !== 46 && charCode !== 38 && charCode !== 47 && charCode !== 40 && charCode !== 41 && charCode !== 32)
        return false;
    return true;

}

//Function to allow ValiDating Company Name
function validateEnter(evt, id) {
    var charCode = (evt.which) ? evt.which : event.keyCode;

    //if (document.getElementById(id).value === '') {
    //    if (evt.which === 32)
    //        return false;
    //}
    if (evt.which === 13) {
        return false;
    }

}

function validateFloatKeyPress(el, evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    var number = el.value.split('.');
    if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    //just one dot (thanks ddlab)
    if (number.length > 1 && charCode == 46) {
        return false;
    }
    //get the carat position
    var caratPos = getSelectionStart(el);
    var dotPos = el.value.indexOf(".");
    if (caratPos > dotPos && dotPos > -1 && (number[1].length > 1)) {
        return false;
    }
    return true;
}

//thanks: http://javascript.nwbox.com/cursor_position/
function getSelectionStart(o) {
    if (o.createTextRange) {
        var r = document.selection.createRange().duplicate();
        r.moveEnd('character', o.value.length);
        if (r.text === '') return o.value.length;
        return o.value.lastIndexOf(r.text);
    } else return o.selectionStart;
}