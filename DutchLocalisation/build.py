import json
import shutil
from pathlib import Path


def build() -> None:
    modname: str = ""
    modversion: str = ""
    with open("manifest.json", "r") as fr:
        data = json.load(fr)
        modname = data["UniqueID"].split(".")[1]
        modversion = data["Version"]

    output_path_str: str = f"./bin/{modname} {modversion}"
    Path(output_path_str).mkdir(parents=True, exist_ok=True)
    output_path = Path(output_path_str)

    Path(f"{output_path}/{modname}").mkdir(parents=True, exist_ok=True)
    copy_path = Path(f"{output_path}/{modname}")
    shutil.copy(Path("./manifest.json"), copy_path)
    shutil.copy(Path("./content.json"), copy_path)
    shutil.copytree(Path("./assets"), Path(f"{copy_path}/assets"), dirs_exist_ok=True)

    Path(f"./bin/__releases").mkdir(parents=True, exist_ok=True)
    release_path = Path(f"./bin/__releases/{modname} {modversion}")
    shutil.make_archive(str(release_path), format="zip", root_dir=output_path)


if __name__ == "__main__":
    build()
