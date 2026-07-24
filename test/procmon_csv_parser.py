from argparse import ArgumentParser
from argparse import Namespace

def main(args: Namespace) -> None:
    column_idx = int(args.columns) - 1  # Convert to zero-based index
    procmon_csv_file = str(args.file)
    unique_values = set()

    try:
        fp = open(procmon_csv_file, "r", encoding="utf-8")
        for line in fp:
            line = line.strip()
            if not line:
                continue  # Skip empty lines
            columns = line.split(",")
            if column_idx < len(columns):
                if columns[column_idx] not in unique_values:
                    unique_values.add(columns[column_idx])
        fp.close()
    except FileNotFoundError:
        print(f"Error: File '{procmon_csv_file}' not found.")
        return

    for value in sorted(unique_values):
        print(value)

if __name__ == "__main__":
    parser = ArgumentParser(description="ProcMon CSV Parser.")
    parser.add_argument(
        "-c", "--columns",
        help="Column to extract and list unique values from.",
        type=str,
        default="4"
    )
    parser.add_argument(
        "-f", "--file",
        help="Path to the ProcMon CSV file.",
        type=str,
        required=True
    )

    args: Namespace = parser.parse_args()
    main(args)