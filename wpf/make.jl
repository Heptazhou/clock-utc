# Copyright (C) 2023-2024 Heptazhou <zhou@0h7z.com>
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

using EzXML: readxml

const csproj(p, x) = findlast("//$x", readxml("$p/$p.csproj")).content

const a = `-m0=lzma -md1024m -mfb273 -mmt -mqs -ms -mtm- -mx9 -stl -xr!\*.pdb`
const c = "Release"
const n = "Clock"
const t = csproj(n, :TargetFramework)
const z = csproj(n, :Product) * '-' * match(r"^\w+", t).match

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
	fmt("$n/bin/$c/$t/")
	mv("$n/bin/$c/$t/", "$n/bin/$c/$z/")
	run(`7z a $a $z.7z ./$n/bin/$c/$z/`)
	rm("$n/bin/$c/$z", recursive = true)
	rm("$n.sln")
catch e
	@info e
end
isempty(ARGS) || exit()
print("\n> ")
readline()

