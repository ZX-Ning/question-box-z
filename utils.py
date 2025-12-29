#!/usr/bin/python3

import argparse
import asyncio

async def read_stream(stream, prefix):
    while True:
        line = await stream.readline()
        if not line:
            break
        print(f"{prefix}: {line.decode('utf-8').strip()}")

async def run_dev():
    node_proc = await asyncio.create_subprocess_exec(
        *["npx", "vite", "--host", "0.0.0.0"],
        stdout=asyncio.subprocess.PIPE,
        stderr=asyncio.subprocess.STDOUT,
        cwd="QuestionBox.Client"
    )
    dotnet_proc = await asyncio.create_subprocess_exec(
        *["dotnet", "run", "-c=debug", "--launch-profile=Development", "--framework=net10.0"],
        stdout=asyncio.subprocess.PIPE,
        stderr=asyncio.subprocess.STDOUT,
        cwd="QuestionBox.Server"
    )

    task1 = asyncio.create_task(read_stream(node_proc.stdout, "[NODE]"))
    task2 = asyncio.create_task(read_stream(dotnet_proc.stdout, "[DOTNET]"))

    await asyncio.gather(task1, task2)
    rc1 = await node_proc.wait()
    rc2 = await dotnet_proc.wait()

    print(f"node returns: {rc1}")
    print(f"dotnet returns: {rc2}")

if __name__ == "__main__":
    print("start..")
    parser = argparse.ArgumentParser(description='Dev utils')
    parser.add_argument('dev', help='Path to the input file.')
    args = parser.parse_args()
    if args.dev == "dev" :
        print("running dev...")
        asyncio.run(run_dev())
    else:
        print("Not supported operation")
