<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="Games.aspx.cs" Inherits="Domotica_ASP.Games" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
        <asp:ListItem>WordScrambler</asp:ListItem>
        <asp:ListItem>Poker</asp:ListItem>
        <asp:ListItem>Snake</asp:ListItem>
    </asp:DropDownList>
    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
        <!-- SOURCE: https://javascriptsource.com/word-scramble-game-593171/ -->
        <asp:View ID="WordScrambler" runat="server">
            <script type="text/javascript">
                <!--
                var word = new Array("VERGROTEN", "PRACHTIG", "DISRESPECT", "SCULPTUUR", "VRESELIJK", "BEVEILIGING", "ADEPT", "AWESOME", "KOE");
                var letter = new Array();
                var rand;
                var actual;
                window.onload = main;
                function main() {
                    getRand();
                    scrambleWord();
                    creatWord();
                }
                function scrambleWord() {
                    actual = word[rand];
                    Temp = word[rand].length;
                    for (var HUCKO = 0; HUCKO < Temp + 2; HUCKO++)// - Richard Hucko's JavaScript Effects -> www.DynamicScripts.tk
                    {
                        RHUCKO1 = HUCKO - 1;
                        letter[HUCKO] = word[rand].substring(RHUCKO1, RHUCKO1 - 1)
                    }
                    letter.sort();
                    word[rand] = "";
                    for (var HUCKO = 0; HUCKO < Temp + 2; HUCKO++)
                        word[rand] += letter[HUCKO];
                }
                function getRand() {
                    rand = Math.floor(Math.random() * word.length);
                }
                function creatWord() {
                    for (var x = 0; x < word[rand].length + 2; x++) {
                        RHUCKO1 = x - 1;
                        a = document.createElement('a');
                        a.id = "aid" + x;
                        a.className = "scramble";
                        a.innerHTML = word[rand].substring(RHUCKO1, RHUCKO1 - 1);
                        a.href = "javascript:append('" + x + "');";
                        document.getElementById('board_').appendChild(a);
                    }
                }
                function creatWord2(x) {
                    RHUCKO1 = x - 1;
                    a = document.createElement('a');
                    a.id = "aid2" + x;
                    a.className = "scramble";
                    a.innerHTML = word[rand].substring(RHUCKO1, RHUCKO1 - 1);
                    a.href = "javascript:retract('" + x + "');";
                    document.getElementById('assemble').appendChild(a);

                }
                function append(RHUCKO1) {
                    document.getElementById('aid' + RHUCKO1).style.visibility = "hidden";
                    //document.getElementById('assemble').innerHTML+=document.getElementById('aid'+RHUCKO1).innerHTML;
                    creatWord2(RHUCKO1);
                }
                function retract(RHUCKO1) {
                    document.getElementById('aid' + RHUCKO1).style.visibility = "visible";
                    elem = document.getElementById('aid2' + RHUCKO1);
                    document.getElementById('assemble').removeChild(elem);
                }
                function showWord() {
                    document.getElementById('theWord').innerHTML = actual;
                }
                //-->
            </script>
            <style type="text/css">
                <!--
                a.scramble {
                    color: black;
                    font-weight: bold;
                    font-size: 1.3em;
                    position: relative;
                }

                span#assemble {
                    font-weight: bold;
                    position: relative;
                }

                span#theWord {
                    color: white;
                    background-color: black;
                    font-weight: bold;
                    position: relative;
                }

                a:hover {
                    background-color: black;
                    color: white;
                }
                -->
            </style>
            <div id="MainDiv1" style="justify-content: center;
                                    background: #d8d8d84a;
                                    width: 70%;
                                    margin-left: 15%;
                                    padding: 0px;
                                    border-radius: 5px;">
                <br />
                <span id="board_"></span>
                <br />
                <span id="assemble"></span>
                <br />
                <span id="theWord"></span>
                <br /><br />
                <form>
                    <input type="button" value="->See Word" onclick="showWord();" />
                    <b style="visibility:hidden">__</b>
                    <input type="button" value="Play Again!" onclick="javascript:history.go(0);" />
                    <div style="text-align:center;color:black;background-color:transparent;display:none;z-index:1;text-decoration:underline;">
                        Word Scramble Cross-Browser JavaScript, Author: Richard Hucko (RHUCKO1), Web Site: www.DynamicScripts.tk, Category: Games, Browsers: All Newer Browsers
                    </div>
                </form>
            </div>
         </asp:View>
        <!-- SOURCE: https://javascriptsource.com/web-poker-386371/ -->
        <asp:View ID="Poker" runat="server">
            <script type="text/javascript">
                <!--
                    /**********************************************************************************************
                    - JavaScript Created By Richard Hucko , Honolulu Hawaii , website: http://geocities.com/rhucko1 - http://rhucko1.50megs.com - http://rhucko1.0catch.com - http://rhucko1.bravehost.com , title: Web Poker Game , 2004 (IE 5.+ , Netscape 7.+ , Mozilla 1.2+)
                    **********************************************************************************************/
                    window.onload = main;
                    function main() {
                        createCards();
                    }
                    ///////////////////////////////////////////////////////////////////////////////
                    var indices = new Array();
                    indices = ["A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K"];
                    var suits = new Array();
                    suits = (navigator.appName == "Netscape") ? [" S", " H", " C", " D"] : ["<br />S", "<br />H", "<br />C", "<br />D"];
                    var spades = new Array("", "", "", "", "", "", "", "", "", "", "", "", ""),
                        hearts = new Array("", "", "", "", "", "", "", "", "", "", "", "", ""),
                        clubs = new Array("", "", "", "", "", "", "", "", "", "", "", "", ""),
                        diamonds = new Array("", "", "", "", "", "", "", "", "", "", "", "", "");
                    var deck = new Array();
                    deck = [spades, hearts, clubs, diamonds];
                    ///////////////////////////////////////////////////////////////////////////////
                    function createCards() {
                        var gcount = 0;

                        while (gcount < 4) {
                            for (var x = 0; x < indices.length; x++) { deck[gcount][x] += indices[x] + suits[gcount]; }
                            gcount++;
                        }
                    }
                    ///////////////////////////////////////////////////////////////////////////////
                    var identify = 0;
                    function deal(num) {
                        randI = Math.floor(Math.random() * suits.length);
                        randS = Math.floor(Math.random() * indices.length);
                        if (deck[randI][randS] != "-") {
                            a = document.createElement('a');
                            a.id = "aid" + identify;
                            a.href = "javascript:discard('" + identify + "')";
                            a.innerHTML = (navigator.appName == "Netscape") ? " --&gt " : "-&gt<br />D<br />-&gt";
                            with (a.style) {
                                fontWeight = "bold";
                                textDecoration = "none";
                                color = "black";
                                height = 70;
                            }

                            spans = document.createElement("spans");
                            spans.id = "sid" + identify;
                            spans.className = "card";
                            spans.innerHTML = (navigator.appName == "Netscape") ? "<b>" + deck[randI][randS] + "<tt style='color:gray'>|</tt></b>" : "<b>" + deck[randI][randS] + "</b>";
                            with (spans.style) {
                                textAlign = "center";
                                width = 80;
                                height = 100;
                                border = "3 inset black";
                                fontSize = 40;
                            }
                            if (spans.innerHTML.indexOf("S") == -1 && spans.innerHTML.indexOf("C") == -1) { spans.style.color = "red"; } else { spans.style.color = "black"; }
                            document.getElementById('MainDiv').appendChild(a);
                            document.getElementById('MainDiv').appendChild(spans);

                            deck[randI][randS] = "-";

                            identify++;
                        }
                        else {
                            redeal();
                        }
                    }
                    var unique = 0;
                    var tcount = 0;
                    function dealersHand() {
                        adapt = Math.floor(Math.random() * 10);
                        if (adapt < 4) {

                        }
                        else if (adapt == 4) {
                            while (tcount < 2) {
                                for (var move = 0; move < 13; move++) {
                                    if (deck[tcount][move] != "-") {
                                        deck[tcount][move] = "-";
                                    }
                                }
                                tcount++;
                            }
                        }
                        else {
                            while (tcount < 4) {
                                for (var move = 1; move <= 7; move++) {
                                    if (deck[tcount][move] != "-") {
                                        deck[tcount][move] = "-";
                                    }
                                }
                                tcount++;
                            }
                        }
                        randI = Math.floor(Math.random() * suits.length);
                        randS = Math.floor(Math.random() * indices.length);
                        if (deck[randI][randS] != "-") {
                            old = document.getElementById('dealer' + unique);
                            spans = document.createElement("spans");
                            spans.id = "sid" + identify;
                            spans.className = "card";
                            spans.innerHTML = (navigator.appName == "Netscape") ? "<b>" + deck[randI][randS] + "<tt style='color:gray'>|</tt></b>" : "<b>" + deck[randI][randS] + "</b>";
                            with (spans.style) {
                                textAlign = "center";
                                width = 50;
                                height = 10;
                                border = "2 inset black";
                                fontSize = 30;
                            }
                            if (spans.innerHTML.indexOf("S") == -1 && spans.innerHTML.indexOf("C") == -1) { spans.style.color = "red"; } else { spans.style.color = "black"; }
                            document.getElementById('MainDiv').replaceChild(spans, old);

                            deck[randI][randS] = "-";

                            unique++;
                        }
                        else {
                            redealDealer();
                        }
                    }

                    function redeal() {
                        deal();
                    }
                    function redealDealer() {
                        dealersHand();
                    }
                    function amt(num) {
                        for (var x = 0; x < num; x++)
                            deal();
                    }
                    var dcount = 0;
                    function discard(i) {
                        document.getElementById('sid' + i).style.display = "none";
                        document.getElementById('aid' + i).style.display = "none";
                        dcount++;
                    }
                    function draw() {
                        for (var x = 0; x < dcount; x++)
                            deal();

                        dcount = 0;
                    }
                    function dealIn() {
                        for (var y = 0; y < 5; y++)
                            dealersHand();

                    }
                    function hide(i) {
                        document.getElementById('caid' + i).style.display = 'none';
                    }
                    function show(i) {
                        document.getElementById('caid' + i).style.display = "block";
                    }
                /**********************************************************************************************
                - JavaScript Created By Richard Hucko , Honolulu Hawaii , website: http://geocities.com/rhucko1 - http://rhucko1.50megs.com - http://rhucko1.0catch.com - http://rhucko1.bravehost.com , title: Web Poker Game , 2004
                **********************************************************************************************/
                //-->
            </script>
            <style type="text/css">
                <!--
                .control {
                    background-color: black;
                    color: white;
                    text-decoration: none;
                    font-weight: bold;
                    display: none;
                    position: absolute;
                    border-top-left-radius: 5px;
                    padding: 3px;
                    border-bottom-right-radius: 10px;
                }

                div {
                    font-weight: bold;
                }
                -->
            </style>
            <div id="MainDiv" style="justify-content: center;
                                    background: #d8d8d84a;
                                    width: 70%;
                                    margin-left: 15%;
                                    padding: 0px;
                                    border-radius: 5px;">
                <a href="javascript:amt(5);hide(0);show(1)" style="display:block" class="control" id="caid0">Deal</a>
                <a href="javascript:draw();hide(1);show(2)" class="control" id="caid1">Draw</a>
                <a href="javascript:dealIn();hide(2);show(3)" class="control" id="caid2">ShowDown</a>
                <a href="javascript:history.go(0)" class="control" id="caid3">Play Again</a>
                <b>Dealer's Hand: </b><br />
                <span id="dealer0"> * </span><span id="dealer1"> * </span><span id="dealer2"> * </span><span id="dealer3"> * </span><span id="dealer4"> * </span><span id="dealer5"> </span><span id="dealer6"> </span><span id="dealer7"> </span>
                <br /><br /><h1>Your Hand:</h1>
            </div>
        </asp:View>
        <!-- SOURCE: https://hjnilsson.com/2018/03/31/snake-in-pure-html-part-1/ -->
        <asp:View ID="snake" runat="server" OnInit="snake_Init">
            <style>
                html {
                    text-align: center;
                    font-family: Helvetica, Arial, Helvetica, sans-serif;
                }

                #board {
                    width: calc(26 * 24px);
                    margin: auto;
                }

                #board div {
                    background-color: black;
                    border: 1px solid grey;
                    box-sizing: border-box;
                    float: left;
                    width: 24px;
                    height: 24px;
                }

                #board .snake {
                    background-color: green;
                }

                #board .apple {
                    background-color: red;
                }
            </style>
            <div id="MainDiv1" style="background: #d8d8d84a;
                                        width: 70%;
                                        padding: 0px;
                                        border-radius: 5px;
                                        margin-left: 15%">
                <span style="font-size: 2em; text-align: center; font-weight: bold">Simple Snake</span>
                <div id="board" style="width: calc(26 * 24px);
                                       height: calc(16*24px);"></div>

                <script type="text/javascript">
                    const board = [];
                    const boardWidth = 26, boardHeight = 16;

                    var snakeX;
                    var snakeY;
                    var snakeLength;
                    var snakeDirection;
                    var speed_game_overtime = true;
                    var start_gamespeed = 200;
                    var gamespeed = start_gamespeed;

                    function initGame() {
                        const boardElement = document.getElementById('board');

                        for (var y = 0; y < boardHeight; ++y) {
                            var row = [];
                            for (var x = 0; x < boardWidth; ++x) {
                                var cell = {};
                
                                // Create a <div></div> and store it in the cell object
                                cell.element = document.createElement('div');
                
                                // Add it to the board
                                boardElement.appendChild(cell.element);
    
                                // Add to list of all
                                row.push(cell);
                            }
    
                            // Add this row to the board
                            board.push(row);
                        }

                        startGame();
                        // Start the game loop (it will call itself with timeout)
                        gameLoop();
                    }

                    function placeApple() {
                        // A random coordinate for the apple
                        var appleX = Math.floor(Math.random() * boardWidth);
                        var appleY = Math.floor(Math.random() * boardHeight);

                        board[appleY][appleX].apple = 1;
                    }
        
                    function startGame() {
                        // Default position for the snake in the middle of the board.
                        snakeX = Math.floor(boardWidth / 2);
                        snakeY = Math.floor(boardHeight / 2);
                        snakeLength = 5;
                        gamespeed = start_gamespeed;
                        snakeDirection = 'Up';

                        // Clear the board
                        for (var y = 0; y < boardHeight; ++y) {
                            for (var x = 0; x < boardWidth; ++x) {
                                board[y][x].snake = 0;
                                board[y][x].apple = 0;
                            }
                        }

                        // Set the center of the board to contain a snake
                        board[snakeY][snakeX].snake = snakeLength;

                        // Place the first apple on the board.
                        placeApple();
                    }

                    function gameLoop() {

                        // Update position depending on which direction the snake is moving.
                        switch (snakeDirection) {
                            case 'Up':    snakeY--; break;
                            case 'Down':  snakeY++; break;
                            case 'Left':  snakeX--; break;
                            case 'Right': snakeX++; break;
                        }

                        // Check for walls, and restart if we collide with any
                        if (snakeX < 0 || snakeY < 0 || snakeX >= boardWidth || snakeY >= boardHeight) {
                            startGame();
                        }

                        // Tail collision
                        if (board[snakeY][snakeX].snake > 0) {
                            startGame();
                        }

                        // Collect apples
                        if (board[snakeY][snakeX].apple === 1) {
                            var _gamespeed;
                            if (speed_game_overtime) {
                                 _gamespeed = gamespeed * snakeLength;
                            }
                            snakeLength++;
                            gamespeed = _gamespeed / snakeLength
                            board[snakeY][snakeX].apple = 0;
                            placeApple();
                        }

                        // Update the board at the new snake position
                        board[snakeY][snakeX].snake = snakeLength;

                        // Loop over the entire board, and update every cell
                        for (var y = 0; y < boardHeight; ++y) {
                            for (var x = 0; x < boardWidth; ++x) {
                                var cell = board[y][x];

                                if (cell.snake > 0) {
                                    cell.element.className = 'snake';
                                    cell.snake -= 1;
                                }
                                else if (cell.apple === 1) {
                                    cell.element.className = 'apple';
                                }
                                else {
                                    cell.element.className = '';
                                }
                            }
                        }

                        // This function calls itself, with a timeout of 1000 milliseconds
                        setTimeout(gameLoop, gamespeed);
                    }

                    function enterKey(event) {
                        // Update direction depending on key hit
                        switch (event.key) {
                            case 'ArrowUp': snakeDirection = 'Up'; break;
                            case 'ArrowDown': snakeDirection = 'Down'; break;
                            case 'ArrowLeft': snakeDirection = 'Left'; break;
                            case 'ArrowRight': snakeDirection = 'Right'; break;
                            default: break;
                        }

                        // This prevents the arrow keys from scrolling the window
                        event.preventDefault();
                    }

                </script>
                <br />
                <span>Source: <a href="https://hjnilsson.com/2018/03/31/snake-in-pure-html-part-1/">Source blog, made by @hjnilsson</a></span>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
