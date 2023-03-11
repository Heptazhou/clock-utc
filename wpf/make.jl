# Copyright (C) 2023 Heptazhou <zhou@0h7z.com>
#
# This program is free software: you can redistribute it and/or modify
# it under the terms of the GNU Affero General Public License as
# published by the Free Software Foundation, either version 3 of the
# License, or (at your option) any later version.
#
# This program is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
# GNU Affero General Public License for more details.
#
# You should have received a copy of the GNU Affero General Public License
# along with this program.  If not, see <https://www.gnu.org/licenses/>.

const a = `-m0=lzma -md1024m -mfb273 -mmt -ms -mx9 -stl -xr!\*.pdb`
const c = "Release"
const f = "net7.0-windows"
const n = "Clock"
const z = "clock-utc-net7"

function fmt(dir::String)
	cd(dir) do
		cp("$(@__DIR__)/notice.txt", "NOTICE.txt")
		r = r"^\t*\K {2}"m
		for f ∈ filter!(endswith(".json"), readdir())
			s = read(f, String)
			s = replace(s, "\r\n" => "\n")
			while contains(s, r)
				s = replace(s, r => "\t")
			end
			isempty(s) || endswith(s, "\n") || (s *= "\n")
			write(f, s)
		end
	end
end

try
	cd(@__DIR__)
	rm("$z.7z", force = true)
	rm("$n.sln", force = true)
	rm("$n/obj/", force = true, recursive = true)
	rm("$n/bin/", force = true, recursive = true)
	run(`dotnet --info`)
	run(`dotnet new sln -n $n`)
	run(`dotnet sln add $(n)`)
	run(`dotnet build -c $c`)
	fmt("$n/bin/$c/$f/")
	mv("$n/bin/$c/$f/", "$n/bin/$c/$n/")
	run(`7z a $a $z.7z ./$n/bin/$c/$n/`)
	rm("$n/bin/$c/$n", recursive = true)
	rm("$n.sln")
catch e
	@info e
end
isempty(ARGS) || exit()
print("\n> ")
readline()

