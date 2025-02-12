App.service('Loader', [
    function ($http) {


        this.Open = () => {
            let Loader = document.getElementById('Loader');
            Loader.style.display = "flex";
            document.body.classList.add('no-scroll');
            //document.body.style.opacity = "0"
        }

        this.Close = async () => {
            //setTimeout(() => {
            let Loader = document.getElementById('Loader');
            Loader.style.display = "none";
            document.body.classList.remove('no-scroll');
            //document.body.style.opacity = "1"
            /* },1000)*/
        }


    }
]);