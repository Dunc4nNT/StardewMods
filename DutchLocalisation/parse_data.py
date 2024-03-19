import datetime
import shutil
from pathlib import Path


def parse() -> None:
    input_path = Path("./data/input/vanilla")
    german_data = list(input_path.glob("**/*.de-DE.*"))

    current_date = datetime.datetime.now(datetime.UTC)
    folderdate = f"{current_date.year}-{current_date.month}-{current_date.day}"

    Path(f"./data/output-{folderdate}/assets/vanilla/").mkdir(parents=True, exist_ok=True)
    outputpath_vanilla = Path(f"./data/output-{folderdate}/assets/vanilla/")

    for file in german_data:
        gamefilepath = str(file.parent).replace("data\\input\\vanilla\\", "").replace("\\", "/")

        if gamefilepath == "Fonts":
            continue

        _ = Path(f"{outputpath_vanilla}/{gamefilepath}").mkdir(parents=True, exist_ok=True),
        shutil.copy(
            Path(str(file).replace(".de-DE", "")),
            Path(f"{outputpath_vanilla}/{gamefilepath}/{file.name.replace(".de-DE", ".nl-NL")}"),
        )

    outputpath = Path(f"./data/output-{folderdate}/assets/")
    outputpath_folders = list(outputpath.glob("**/"))
    
    for path in outputpath_folders:
        Path(f"{path}/__content.json").touch()

    for path in outputpath_folders:
        content_file = Path(f"{path}/__content.json")
        with open(content_file, "w") as fw:
            fw.writelines([
                "{\n",
                '  "$schema": "https://smapi.io/schemas/content-patcher.json",\n',
                '  "Changes": [\n'
            ])

            jsonfiles: list[Path] = []
            for sub in path.iterdir():
                if sub.is_dir():
                    jsonfiles += (list(sub.glob("__content.json")))
            jsonfiles += list(path.glob("*.json"))

            for file in jsonfiles:
                if file == content_file:
                    continue

                fw.writelines([
                    "    {\n",
                    '      "Action": "Include",\n',
                    f'      "FromFile": "assets{str(Path(str(file).replace(str(outputpath), ""))).replace("\\", "/")}"\n',
                    "    },\n"
                ])
            
            fw.writelines(["  ]\n", "}\n"])


if __name__ == "__main__":
    parse()
