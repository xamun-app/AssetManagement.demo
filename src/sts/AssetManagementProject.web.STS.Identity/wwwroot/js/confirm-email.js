var count = 6;
window.onload = countdown();
function countdown() {
    count--;
    if (count == 0) {
        window.location.href = 'https://localhost:4200';
    }
    else {
        document.getElementById("count").innerHTML = "" + count;
        setTimeout(countdown, 1000);
    }
};
