$(function () {
    $.get("billett/SjekkLoggetIn", function () {
    }).fail(function (feil) {
        if (feil.status == 401) {
            window.location.href = 'loggInn.html';  // ikke logget inn, redirect til loggInn.html
        }
        else {
            $("#feil").html("Feil på server - prøv igjen senere");
        }
    });
});

function leggTil() {

    validerPris();
    validerAngangInput();
    validerNavn();
    validerTidInput();

    if (validerNavn() && validerAngangInput() && validerPris() && validerTidInput()) {
        // må ha med id inn skjemaet, hidden i html
        let listAvgangsTider =
            [$("#settid1").val(), $("#settid2").val(),
            $("#settid3").val(), $("#settid4").val(),
            $("#settid5").val(), $("#settid6").val()];

        let listAvganger =
            [$("#avg1").val(), $("#avg2").val(),
            $("#avg3").val(), $("#avg4").val(),
            $("#avg5").val(), $("#avg6").val()];

        listAvgangsTider = listAvgangsTider.join(',');
        listAvganger = listAvganger.join(',');

        const Rute =
        {
            BussNavn: $("#buss").val(),
            Pris: $("#pris").val(),
            Avganger: listAvganger,
            Tider: listAvgangsTider
        }

        $.post("billett/LeggTilRute", Rute, function (OK) {
            if (OK) {
                alert("Lagt til rute. NB! Denne vil også lage retur bussen");
                window.location.href = 'admin.html';
            }
            else {
                alert("Feil i db");
            }
        }).fail(function (feil) {
            if (feil.status == 401) {
                window.location.href = 'loggInn.html';  // ikke logget inn, redirect til loggInn.html
            }
            else {
                $("#feil").html("Feil på server - prøv igjen senere");
            };

        });
    }
}
function loggut() {
    $.get("billett/LoggUt", function () {
        window.location.href = "/logginn.html";
    });
}






function validerNavn() {
    const buss = $("#buss").val();
    const regexp = /^[a-zæøåA-ZÆØÅ.\-_ ]{2,20}$/;
    const ok = regexp.test(buss);
    if (!ok) {
        $("#message0").css('color', 'red');
        $("#message0").html("Ugjyldig navn!");
        return false;
    }
    else {
        $("#message0").html("");
        return true;
    }
}

function validerPris() {
    const pris = $("#pris").val();
    const regexp = /^(\d*([.,](?=\d{3}))?\d+)+((?!\2)[.,]\d\d)?$/;
    const ok = regexp.test(pris);
    if (!ok) {
        $("#message1").css('color', 'red');
        $("#message1").html("Ugjyldig Pris!");
        return false;
    }
    else {
        $("#message1").html("");
        return true;
    }
}

function validerAngangInput() {
    const avg1 = $("#avg1").val();
    const avg2 = $("#avg2").val();
    const avg3 = $("#avg3").val();
    const avg4 = $("#avg4").val();
    const avg5 = $("#avg5").val();
    const avg6 = $("#avg6").val();
    const regexp = /^[a-zæøåA-ZÆØÅ.\- ]{2,20}$/;
    const ok = regexp.test(avg1);
    const ok1 = regexp.test(avg2);
    const ok2 = regexp.test(avg3);
    const ok3 = regexp.test(avg4);
    const ok4 = regexp.test(avg5);
    const ok5 = regexp.test(avg6);
    let godkjent = false;
    if (!ok || avg1 == "") {
        $("#message2").css('color', 'red');
        $("#message2").html("Ugjyldig value!");
        godkjent = false;
    }
    else {
        $("#message2").html("");
        godkjent = true;
    }
    if (!ok1 || avg2 == "") {
        $("#message3").css('color', 'red');
        $("#message3").html("Ugjyldig value!");
        godkjent = false;
    }

    else {
        $("#message3").html("");
        godkjent = true;
    }
    if (!ok2 || avg3 == "") {
        $("#message4").css('color', 'red');
        $("#message4").html("Ugjyldig value!");
        godkjent = false;
    }
    else {
        $("#message4").html("");
        godkjent = true;
    }
    if (!ok3 || avg4 == "") {
        $("#message5").css('color', 'red');
        $("#message5").html("Ugjyldig value!");
        godkjent = false;
    }
    else {
        $("#message5").html("");
        godkjent = true;
    }
    if (!ok4 || avg5 == "") {
        $("#message6").css('color', 'red');
        $("#message6").html("Ugjyldig value!");
        godkjent = false;
    }
    else {
        $("#message6").html("");
        godkjent = true;
    }
    if (!ok5 || avg6 == "") {
        $("#message7").css('color', 'red');
        $("#message7").html("Ugjyldig value!");
        godkjent = false;
    }
    else {
        $("#message7").html("");
        godkjent = true;
    }
    return godkjent;
}

function validerTidInput() {

    var godkjent = false;
    const avg1 = $("#settid1").val();
    const avg2 = $("#settid2").val();
    const avg3 = $("#settid3").val();
    const avg4 = $("#settid4").val();
    const avg5 = $("#settid5").val();
    const avg6 = $("#settid6").val();
    const regexp = /^([0-9]{2})\:([0-9]{2})$/;
    const ok = regexp.test(avg1);
    const ok1 = regexp.test(avg2);
    const ok2 = regexp.test(avg3);
    const ok3 = regexp.test(avg4);
    const ok4 = regexp.test(avg5);
    const ok5 = regexp.test(avg6);
    if (!ok || avg1 == null) {
        $("#message8").css('color', 'red');
        $("#message8").html("Ugjyldig value!");
        godkjent = false;
    }

    else {
        $("#message8").html("");
        godkjent = true;
    }
    if (!ok1 || avg2 == null) {
        $("#message9").css('color', 'red');
        $("#message9").html("Ugjyldig value!");
        godkjent = false;
    }

    else {
        $("#message9").html("");
        godkjent = true;
    }

    if (!ok2 || avg3 == null) {
        $("#message10").css('color', 'red');
        $("#message10").html("Ugjyldig value!");
        godkjent = false;
    }
    else {
        $("#message10").html("");
        godkjent = true;
    }
    if (!ok3 || avg4 == null) {
        $("#message11").css('color', 'red');
        $("#message11").html("Ugjyldig value!");
        godkjent = false;
    }
    else {
        $("#message11").html("");
        godkjent = true;
    }
    if (!ok4 || avg5 == null) {
        $("#message12").css('color', 'red');
        $("#message12").html("Ugjyldig value!");
        godkjent = false;
    }
    else {
        $("#message12").html("");
        godkjent = true;
    }
    if (!ok5 || avg6 == null) {
        $("#message13").css('color', 'red');
        $("#message13").html("Ugjyldig value!");
        godkjent = false;
    }
    else {
        $("#message13").html("");
        godkjent = true;
    }

    return godkjent;

}