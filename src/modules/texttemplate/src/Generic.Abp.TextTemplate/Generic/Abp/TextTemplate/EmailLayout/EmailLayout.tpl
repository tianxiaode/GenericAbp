<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title></title>
    <style>
        .bg-page {
            background-color: darkgray;
        }

        .text-center {
            text-align: center;
        }

        .text-left {
            text-align: left
        }

        .container {
            width: 500px;
            display: block;
            margin: auto;
        }

        .company {
            padding: 10px;
            font-size: 1.5em;
            background-color: ghostwhite;
        }

        .content {
            display: inline-block;
            padding: 10px;
            min-height: 500px;
            width: 480px;
            background-color: white;
        }

        .verification-code {
            font-weight: 700;
            font-size: 24px;
            line-height: 1.167;
        }
    </style>
</head>
<body class="bg-page text-center">
    {{content}}
</body>
</html>