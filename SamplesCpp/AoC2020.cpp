#include <iostream>
#include <string>
#include <queue>
#include <deque>
#include <bitset>
#include <tuple>
#include <sstream>
#include <regex>
#include <vector>
#include <functional>
#include <algorithm>
#include <map>
#include <set>
#include <stack>
#include <unordered_map>
#include <unordered_set>

using namespace std;
using str = string;

using u1 = uint8_t;
using u2 = uint16_t;
using u4 = uint32_t;
using u8 = uint64_t;
template<typename T>
using v = vector<T>;
template<typename T>
using q = queue<T>;
template<typename T>
using dq = deque<T>;

using TT = u2;

#define R(TV, var) TV var; cin >> var;
#define RV(TV, var, cnt) v<TV> var; var.resize(cnt); for (size_t tmpCnt = 0; tmpCnt < cnt; tmpCnt++) cin >> var[tmpCnt];
#define RQ(TV, var, cnt) q<TV> var; { TV RQTemp; for (size_t tmpCnt = 0; tmpCnt < cnt; tmpCnt++) { cin >> RQTemp; var.push(RQTemp); } }
#define RDQ(TV, var, cnt) dq<TV> var; { TV RQTemp; for (size_t tmpCnt = 0; tmpCnt < cnt; tmpCnt++) { cin >> RQTemp; var.push_back(RQTemp); } }
#define FOR(TV, var, cnt) for (TV var = 0; var < cnt; var++)
#define FOR1(TV, var, cnt0, cnt) for (TV var = cnt0; var < cnt; var++)

#define p(x) std::cout << x << std::endl
#define d(x) // std::cout << x << std::endl

namespace NAoC2020
{
  void Day01_a()
  {
    static constexpr u2 c_wSum = 2020;
    unordered_set<u2> vInput;
    for (u2 i; cin >> i; )
    {
      u4 wDiff = c_wSum - i;
      if (vInput.count(wDiff))
      {
        cout << wDiff << " * " << i << " = " << (wDiff * i);
        // 1477 * 543 = 802011
        return;
      }

      vInput.emplace(i);
    }

    cout << "Not found.";
  }

  void Day01_b()
  {
    static constexpr u2 c_wSum = 2020;
    multiset<u2> vInput;
    for (u2 i; cin >> i; )
    {
      u2 wDiff = c_wSum - i;

      auto i1 = vInput.cbegin();
      auto i1End = vInput.cend();

      auto i2 = vInput.crbegin();
      auto i2End = vInput.crend();

      for (; i1 != i1End; i1++)
      {
        d("i1: " << *i1);

        if (*i1 > wDiff)
        {
          break;
        }

        u2 wDiff2 = wDiff - *i1;

        for (; i2 != i2End; i2++)
        {
          d("i2: " << *i2);
          if (*i2 < wDiff2)
          {
            break;
          }
          if (&*i1 == &*i2 || *i2 > wDiff2)
          {
            continue;
          }

          if (*i2 == wDiff2)
          {
            cout << *i1 << " * " << *i2 << " * " << i << " = " << (static_cast<u4>(*i1) * *i2 * i);
            // 422 * 577 * 1021 = 248607374
            return;
          }
        }
      }

      vInput.emplace(i);
    }

    cout << "Not found.";
  }

  void Day01_b2()
  {
    static constexpr u2 c_wSum = 2020;
    v<u2> vInput;
    for (u2 i; cin >> i; )
    {
      u2 wDiff = c_wSum - i;
      for (u2 i1 : vInput)
      {
        u2 wDiff2 = wDiff - i1;
        for (u2 i2 : vInput)
        {
          if (i2 == wDiff2)
          {
            cout << i1 << " * " << i2 << " * " << i << " = " << (static_cast<u4>(i1) * i2 * i);
            return;
          }
        }
      }

      vInput.emplace_back(i);
    }

    cout << "Not found.";
  }

  void Day02_a()
  {
    regex pat{ R"((\d+)-(\d+) (\w): (\w+))" };
    for (str line; getline(cin, line) && line.size(); )
    {
      smatch matches;
      if (!regex_match(line, matches, pat))
      {
        p("invalid input: " << line);
        continue;
      }
      if (!matches.ready() || matches.size() != 5)
      {
        p("ready: " << matches.ready() << ", size: " << matches.size());
        continue;
      }

      // ...
    }
  }

  void Day02_a2()
  {
    char cTmp;
    u2 wLb;
    u2 wUb;
    char cChar;
    str strPass;

    u2 wCnt = 0;

    while (cin >> wLb)
    {
      cin >> cTmp;
      cin >> wUb;
      cin >> cChar;
      cin >> cTmp;
      cin >> strPass;

      size_t n = count(strPass.begin(), strPass.end(), cChar);
      if (wLb <= n && wUb >= n)
      {
        wCnt++;
      }
    }

    p("res: " << wCnt);
    // res: 586
  }

  void Day02_b()
  {
    char cTmp;
    u2 wLb;
    u2 wUb;
    char cChar;
    str strPass;

    u2 wCnt = 0;

    while (cin >> wLb)
    {
      cin >> cTmp;
      cin >> wUb;
      cin >> cChar;
      cin >> cTmp;
      cin >> strPass;

      if ((strPass[wLb - 1] == cChar) ^ (strPass[wUb - 1] == cChar))
      {
        wCnt++;
      }
    }

    p("res: " << wCnt);
    // res: 352
  }

  void Day03_a()
  {
    size_t sOffset = 0;
    u2 wCnt = 0;

    for (str line; getline(cin, line) && line.size(); )
    {
      if (line[sOffset] == '#')
      {
        wCnt++;
      }

      sOffset += 3;
      if (sOffset >= line.size())
      {
        sOffset -= line.size();
      }
    }

    p("cnt: " << wCnt);
  }

  void Day03_b()
  {
    struct SMove
    {
      const bool mc_bEveryLine;
      const u2 mc_wStep;
      bool m_bUse = true;
      size_t m_sOffset = 0;
      u2 m_wCnt = 0;
    };
    SMove Moves[5] = { { 1, 1, }, { 1,  3, }, { 1,  5, }, { 1, 7, }, { 0, 1, } };

    for (str line; getline(cin, line) && line.size(); )
    {
      for (SMove& rMove : Moves)
      {
        if ((rMove.mc_bEveryLine || rMove.m_bUse) && line[rMove.m_sOffset] == '#')
        {
          rMove.m_wCnt++;
        }

        rMove.m_sOffset += (rMove.mc_bEveryLine || rMove.m_bUse) * rMove.mc_wStep;
        rMove.m_bUse = !rMove.m_bUse;

        if (rMove.m_sOffset >= line.size())
        {
          rMove.m_sOffset -= line.size();
        }
      }
    }

    u8 dwMult = 1;
    for (const SMove& rMove : Moves)
    {
      dwMult *= rMove.m_wCnt;
    }

    p("cnt: " << dwMult);
  }

  void Day04_a()
  {
    u2 wCnt = 0;

    constexpr u2 c_wFields = 8;
    static const str vstrFields[c_wFields]
    {
      "cid",
      "byr",
      "iyr",
      "eyr",
      "hgt",
      "hcl",
      "ecl",
      "pid",
    };
    regex r{ R"(\w+)" };
    regex ar[1]{ regex{R"(\w+)"} };
    static const regex vrRules[c_wFields]
    {
      regex{ R"(\w+)", },
      regex{R"(19[2-9]\d|200[0-2])", },
      regex{R"(201[0-9]|2020)", },
      regex{R"(202[0-9]|2030)", },
      regex{R"((1[5-8][0-9]|19[0-3])cm|(59|6[0-9]|7[0-6])in)", },
      regex{R"(#[0-9a-fA-F]{6})", },
      regex{R"(amb|blu|brn|gry|grn|hzl|oth)", },
      regex{R"(\d{9})", },
    };
    bitset<c_wFields> bsRequired = 1;

    str part;
    for (str line; getline(cin, line); )
    {
      if (line.empty())
      {
        wCnt += bsRequired.all();
        bsRequired = 1;
        continue;
      }
      if (line == "end")
      {
        break;
      }

      istringstream iss(line);
      while (iss >> part)
      {
        for (u2 i = 0; i < c_wFields; i++)
        {
          if (part.substr(0, 3) == vstrFields[i])
          {
            if (regex_match(part.substr(4), vrRules[i]))
            {
              bsRequired.set(i);
            }
            break;
          }
        }
      }
    }

    p("cnt: " << wCnt);
    // cnt: 172
  }

  void Day05_a()
  {
    u2 wMax = 0;
    static constexpr u1 c_bChars = 7 + 3;
    bitset<c_bChars> bsTmp;

    for (str line; getline(cin, line) && line.size(); )
    {
      for (u1 i = 0; i < c_bChars; i++)
      {
        bsTmp.set(c_bChars - i - 1, line[i] == 'B' || line[i] == 'R');
      }

      if (bsTmp.to_ulong() > wMax)
      {
        wMax = static_cast<u2>(bsTmp.to_ulong());
      }

      bsTmp.reset();
    }

    p("max: " << wMax);
    // max: 963
  }

  void Day05_b()
  {
    u2 wMax = 0;
    static constexpr u1 c_bChars = 7 + 3;
    static constexpr u2 c_wChairs = 1 << c_bChars;

    bitset<c_bChars> bsTmp;
    bitset<c_wChairs> bsAll;

    for (str line; getline(cin, line) && line.size(); )
    {
      for (u1 i = 0; i < c_bChars; i++)
      {
        bsTmp.set(c_bChars - i - 1, line[i] == 'B' || line[i] == 'R');
      }

      if (bsTmp.to_ulong() > wMax)
      {
        wMax = static_cast<u2>(bsTmp.to_ulong());
      }

      bsAll.set(bsTmp.to_ulong());

      bsTmp.reset();
    }

    bool bEmpty = true;
    for (u2 i = 0; i < c_wChairs; i++)
    {
      if (bEmpty)
      {
        if (bsAll[i])
        {
          bEmpty = false;
        }
      }
      else
      {
        if (!bsAll[i])
        {
          p("max 2: " << i);
          // max 2: 592
          return;
        }
      }
    }

    p("error");
  }

  void Day06_a()
  {
    bitset<26> bsQs;
    u4 dwTotal = 0;

    bool bEmpty = false;
    while (true)
    {
      for (str line; getline(cin, line); )
      {
        if (line.size())
        {
          bEmpty = false;

          for (char c : line)
          {
            bsQs.set(c - 'a');
          }
          continue;
        }

        if (bEmpty)
        {
          p("sum: " << dwTotal);
          // sum: 6587
          return;
        }

        dwTotal += bsQs.count();
        bsQs.reset();
        bEmpty = true;
      }
    }
  }

  void Day06_b()
  {
    bitset<26> bsQsG;
    bsQsG.set();

    bitset<26> bsQsP;
    u4 dwTotal = 0;

    bool bEmpty = false;
    while (true)
    {
      for (str line; getline(cin, line); )
      {
        if (line.size())
        {
          bEmpty = false;

          for (char c : line)
          {
            bsQsP.set(c - 'a');
          }

          bsQsG &= bsQsP;
          bsQsP.reset();

          continue;
        }

        if (bEmpty)
        {
          p("sum2: " << dwTotal);
          // sum2: 3235
          return;
        }

        dwTotal += bsQsG.count();
        bsQsG.set();
        bEmpty = true;
      }
    }
  }

  void Day07_a()
  {
    regex rBag{ R"(([\w ]+) bags contain ([\w ,]+)\.)" };
    regex rContent{ R"(\d+ ([\w ]+) bags?)" };

    using uss = unordered_set<str>;
    unordered_map<str, uss> bags;

    for (str line; getline(cin, line) && line.size(); )
    {
      smatch matches;
      if (!regex_match(line, matches, rBag))
      {
        p("invalid input: " << line);
        return;
      }
      if (!matches.ready() || matches.size() != 3)
      {
        p("ready: " << matches.ready() << ", size: " << matches.size());
        return;
      }

      bags.emplace(matches[1], uss{});
      if (matches[2] == "no other bags")
      {
        continue;
      }

      smatch content;
      str strBags = matches[2];
      while (regex_search(strBags, content, rContent))
      {
        bags[content[1]].emplace(matches[1]);
        strBags = content.suffix();
      }
    }

    const str strMy = "shiny gold";
    queue<str> qToCheck;
    qToCheck.emplace(strMy);
    unordered_set<str> sPoss;
    while (qToCheck.size())
    {
      str strToCheck = qToCheck.front();
      const uss& rParents = bags[strToCheck];
      qToCheck.pop();

      for (const str& rstrBag : rParents)
      {
        if (!sPoss.count(rstrBag))
        {
          sPoss.emplace(rstrBag);
          qToCheck.emplace(rstrBag);
        }
      }
    }

    p("poss: " << sPoss.size());
    // poss: 139
  }

  void Day07_b()
  {
    regex rBag{ R"(([\w ]+) bags contain ([\w ,]+)\.)" };
    regex rContent{ R"((\d+) ([\w ]+) bags?)" };

    struct SBagCnt
    {
      u4 m_dwCnt;
      str m_strBag;
    };
    using vbc = vector<SBagCnt>;
    struct SBagCon
    {
      u8 m_qwBags = -1;
      vbc m_BagList;
    };
    unordered_map<str, SBagCon> bags;

    for (str line; getline(cin, line) && line.size(); )
    {
      smatch matches;
      if (!regex_match(line, matches, rBag))
      {
        p("invalid input: " << line);
        return;
      }
      if (!matches.ready() || matches.size() != 3)
      {
        p("ready: " << matches.ready() << ", size: " << matches.size());
        return;
      }

      if (matches[2] == "no other bags")
      {
        bags.emplace(matches[1], SBagCon{ 0, });
      }
      else
      {
        smatch content;
        str strBags = matches[2];
        while (regex_search(strBags, content, rContent))
        {
          bags[matches[1]].m_BagList.push_back({ std::stoul(content[1].str().c_str()), content[2].str(), });
          strBags = content.suffix();
        }
      }
    }

    const str strMy = "shiny gold";
    stack<str> sToCheck;
    sToCheck.emplace(strMy);

    while (sToCheck.size())
    {
      str strToCheck = sToCheck.top();
      SBagCon& rCon = bags[strToCheck];

      if (rCon.m_qwBags == -1)
      {
        bool bCanCalc = true;
        u8 qwCnt = 0;
        for (const SBagCnt& rBagCnt : rCon.m_BagList)
        {
          const SBagCon& rChildCon = bags[rBagCnt.m_strBag];
          if (rChildCon.m_qwBags == -1)
          {
            sToCheck.push(rBagCnt.m_strBag);
            bCanCalc = false;
          }
          else
          {
            qwCnt += rBagCnt.m_dwCnt * (1 + rChildCon.m_qwBags);
          }
        }

        if (bCanCalc)
        {
          rCon.m_qwBags = qwCnt;
        }
      }

      if (rCon.m_qwBags != -1)
      {
        sToCheck.pop();
      }
    }

    p("bags: " << bags[strMy].m_qwBags);
    // bags: 58175
  }

  void Day08_a()
  {
    enum class EOp
    {
      Nop,
      Acc,
      Jmp,
    };
    struct SInstr
    {
      EOp m_eOp;
      int32_t m_iArg;
      bool m_bVisited = false;
    };
    v<SInstr> vInstr;

    for (str strOp; cin >> strOp && strOp != "e"; )
    {
      int32_t iArg;
      cin >> iArg;
      vInstr.push_back({ strOp == "nop" ? EOp::Nop : strOp == "acc" ? EOp::Acc : EOp::Jmp, iArg, });
    }

    u8 qwAcc = 0;
    for (SInstr* pIns = &vInstr.front(); !pIns->m_bVisited; )
    {
      pIns->m_bVisited = true;

      switch (pIns->m_eOp)
      {
      case EOp::Nop:
        pIns++;
        continue;
      case EOp::Acc:
        qwAcc += pIns->m_iArg;
        pIns++;
        continue;
      case EOp::Jmp:
        pIns += pIns->m_iArg;
        continue;
      }
    }

    p("Acc: " << qwAcc);
    // Acc: 1753
  }

  void Day08_b()
  {
    enum class EOp
    {
      Nop,
      Acc,
      Jmp,
    };
    struct SInstr
    {
      EOp m_eOp;
      int32_t m_iArg;
      bool m_bVisited = false;
    };
    v<SInstr> vInstr;

    for (str strOp; cin >> strOp && strOp != "e"; )
    {
      int32_t iArg;
      cin >> iArg;
      vInstr.push_back({ strOp == "nop" ? EOp::Nop : strOp == "acc" ? EOp::Acc : EOp::Jmp, iArg, });
    }

    for (SInstr* pInsCh = &vInstr.front(); ; pInsCh++)
    {
      if (pInsCh->m_eOp == EOp::Acc)
      {
        continue;
      }

      pInsCh->m_eOp = pInsCh->m_eOp == EOp::Jmp ? EOp::Nop : EOp::Jmp;

      int64_t llAcc = 0;
      for (SInstr* pIns = &vInstr.front(); pIns <= &vInstr.back() && !pIns->m_bVisited; )
      {
        d(pIns - &vInstr[0]);

        pIns->m_bVisited = true;

        switch (pIns->m_eOp)
        {
        case EOp::Nop:
          pIns++;
          break;
        case EOp::Acc:
          llAcc += pIns->m_iArg;
          pIns++;
          break;
        case EOp::Jmp:
          pIns += pIns->m_iArg;
          break;
        }

        // if (pIns == &vInstr.back() + 1)
        if (pIns > &vInstr.back())
        {
          p("Acc2: " << llAcc);
          return;
          // Acc2: 733
        }
      }

      for (SInstr* pIns = &vInstr.front(); pIns->m_bVisited; )
      {
        d(pIns - &vInstr[0]);

        pIns->m_bVisited = false;

        switch (pIns->m_eOp)
        {
        case EOp::Nop:
          pIns++;
          break;
        case EOp::Acc:
          llAcc += pIns->m_iArg;
          pIns++;
          break;
        case EOp::Jmp:
          pIns += pIns->m_iArg;
          break;
        }
      }

      pInsCh->m_eOp = pInsCh->m_eOp == EOp::Jmp ? EOp::Nop : EOp::Jmp;
    }
  }

  constexpr u2 wPreamble09a = 25;
  bool Find09a(const dq<u8>& rq, u8 val)
  {
    FOR(u4, i, wPreamble09a)
    {
      FOR1(u4, j, i + 1, wPreamble09a)
      {
        if (rq[i] + rq[j] == val)
        {
          return 1;
        }
      }
    }

    return 0;
  }
  void Day09_a()
  {
    RDQ(u8, qqwInput, wPreamble09a);

    for (u8 qwInput; cin >> qwInput; )
    {
      if (!Find09a(qqwInput, qwInput))
      {
        p("Corrupt: " << qwInput);
        // Corrupt: 23278925
        return;
      }

      qqwInput.pop_front();
      qqwInput.push_back(qwInput);
    }

    p("end?");
  }

  void Day09_b()
  {
    constexpr u8 sum = 23278925;
    RDQ(u8, qqwInput, 1);

    for (u8 qwInput; cin >> qwInput; )
    {
      qqwInput.push_back(qwInput);
      if (qwInput >= sum)
      {
        continue;
      }
      qwInput = sum - qwInput;
      for (size_t i = qqwInput.size() - 2; i != -1; i--)
      {
        if (qqwInput[i] == qwInput)
        {
          u8 qwMin = qqwInput[i];
          u8 qwMax = qqwInput[i];
          for (size_t j = i + 1; j < qqwInput.size(); j++)
          {
            if (qqwInput[j] < qwMin)
            {
              qwMin = qqwInput[j];
            }
            if (qqwInput[j] > qwMax)
            {
              qwMax = qqwInput[j];
            }
          }
          p("Sum: " << qwMin + qwMax);
          // Sum: 4011064
          return;
        }
        if (qqwInput[i] > qwInput)
        {
          break;
        }
        qwInput -= qqwInput[i];
      }
    }

    p("end?");
  }

  void Day10_a()
  {
    set<u4> sdwInput;

    for (u4 dwInput; cin >> dwInput; )
    {
      sdwInput.emplace(dwInput);
    }

    u4 adwDiff[3]{ 0, 0, 1, };
    u4 dwPrew = 0;
    for (u4 dwCurr : sdwInput)
    {
      u4 diff = dwCurr - dwPrew;
      if (diff == 0 || diff > 3)
      {
        p("diff? " << diff);
        return;
      }

      adwDiff[diff - 1]++;
      dwPrew = dwCurr;
    }

    p("mult: " << adwDiff[0] * adwDiff[2]);
    // mult: 1625
  }

  void Day10_b()
  {
    map<u4, u8> mInput;
    mInput.emplace(0, 1);

    for (u4 dwInput; cin >> dwInput; )
    {
      mInput.emplace(dwInput, 0);
    }

    for (auto iPair = std::next(mInput.begin()); iPair != mInput.end(); iPair++)
    {
      for (auto iPair2 = std::prev(iPair); iPair2->first + 3 >= iPair->first; iPair2--)
      {
        iPair->second += iPair2->second;

        if (iPair2 == mInput.begin())
        {
          break;
        }
      }
    }

    p("last: " << mInput.rbegin()->second);
    // last: 3100448333024
  }

  void Day11_a()
  {
    v<str> vstrGrid;
    for (str strLine; cin >> strLine && strLine != "e"; )
    {
      vstrGrid.emplace_back(strLine);
    }

    size_t sX = vstrGrid[0].size();
    size_t sY = vstrGrid.size();
    v<str> vstrNext = vstrGrid;

    function<bool(size_t, size_t)> fpIsOccupied = [sX, sY, &vstrGrid](size_t x, size_t y)
    {
      if (x == sX || x == -1 || y == sY || y == -1)
        return false;
      return vstrGrid[y][x] == '#';
    };

    for (bool bModified = true; bModified; )
    {
      bModified = false;

      FOR(size_t, x, sX)
      {
        FOR(size_t, y, sY)
        {
          if (vstrGrid[y][x] == '.')
          {
            continue;
          }

          uint8_t occ = (uint8_t)fpIsOccupied(x - 1, y - 1) + fpIsOccupied(x + 0, y - 1) + fpIsOccupied(x + 1, y - 1) +
            fpIsOccupied(x - 1, y + 0) + fpIsOccupied(x + 1, y + 0) +
            fpIsOccupied(x - 1, y + 1) + fpIsOccupied(x + 0, y + 1) + fpIsOccupied(x + 1, y + 1);

          if (vstrGrid[y][x] == 'L')
          {
            if (!occ)
            {
              vstrNext[y][x] = '#';
              bModified = true;
            }
            continue;
          }

          if (vstrGrid[y][x] == '#')
          {
            if (occ >= 4)
            {
              vstrNext[y][x] = 'L';
              bModified = true;
            }
            continue;
          }
        }
      }

      vstrGrid = vstrNext;
    }

    u4 dwTaken = 0;
    FOR(size_t, x, sX)
    {
      FOR(size_t, y, sY)
      {
        dwTaken += vstrGrid[y][x] == '#';
      }
    }

    p("taken: " << dwTaken);
    // taken: 2438
  }

  void Day11_b()
  {
    v<str> vstrGrid;
    for (str strLine; cin >> strLine && strLine != "e"; )
    {
      vstrGrid.emplace_back(strLine);
    }

    size_t sX = vstrGrid[0].size();
    size_t sY = vstrGrid.size();
    size_t x;
    size_t y;
    v<str> vstrNext = vstrGrid;

    function<bool(size_t, size_t)> fpIsOccupied = [sX, sY, &x1 = x, &y1 = y, &vstrGrid](size_t xOf, size_t yOf)
    {
      size_t x = x1;
      size_t y = y1;
      while (true)
      {
        x += xOf;
        y += yOf;

        if (x == sX || x == -1 || y == sY || y == -1)
          return false;

        if (vstrGrid[y][x] == '.') continue;

        return vstrGrid[y][x] == '#';
      }
    };

    for (bool bModified = true; bModified; )
    {
      bModified = false;

      for (x = 0; x < sX; x++)
      {
        for (y = 0; y < sY; y++)
        {
          if (vstrGrid[y][x] == '.')
          {
            continue;
          }

          uint8_t occ = (uint8_t)fpIsOccupied(-1, -1) + fpIsOccupied(+0, -1) + fpIsOccupied(+1, -1) +
            fpIsOccupied(-1, +0) + fpIsOccupied(+1, +0) +
            fpIsOccupied(-1, +1) + fpIsOccupied(+0, +1) + fpIsOccupied(+1, +1);

          if (vstrGrid[y][x] == 'L')
          {
            if (!occ)
            {
              vstrNext[y][x] = '#';
              bModified = true;
            }
            continue;
          }

          if (vstrGrid[y][x] == '#')
          {
            if (occ >= 5)
            {
              vstrNext[y][x] = 'L';
              bModified = true;
            }
            continue;
          }
        }
      }

      vstrGrid = vstrNext;

      for (const str& rLine : vstrGrid)
      {
        d(rLine);
      }
      d("");
    }

    u4 dwTaken = 0;
    FOR(size_t, x, sX)
    {
      FOR(size_t, y, sY)
      {
        dwTaken += vstrGrid[y][x] == '#';
      }
    }

    p("taken: " << dwTaken);
    // taken: 2174
  }

  void Day12_a()
  {
    static constexpr char c_N = 'N';
    static constexpr char c_E = 'E';
    static constexpr char c_S = 'S';
    static constexpr char c_W = 'W';
    static constexpr char c_L = 'L';
    static constexpr char c_R = 'R';
    static constexpr char c_F = 'F';

    int32_t lN = 0;
    int32_t lE = 0;
    u1 bDir = 1;

    u4 dwValue;
    for (char c; cin >> c && c != 'e'; )
    {
      cin >> dwValue;

      switch (c)
      {
      case c_N: lN += dwValue; break;
      case c_E: lE += dwValue; break;
      case c_S: lN -= dwValue; break;
      case c_W: lE -= dwValue; break;
      case c_L: bDir -= dwValue / 90; break;
      case c_R: bDir += dwValue / 90; break;
      case c_F:
      {
        switch (bDir)
        {
        case 0: lN += dwValue; break;
        case 1: lE += dwValue; break;
        case 2: lN -= dwValue; break;
        case 3: lE -= dwValue; break;
        default: p("whooot dir"); return;
        }
      }
      break;
      default: p("whooot c"); return;
      }

      bDir &= 0x3;
    }

    if (lN < 0) lN *= -1;
    if (lE < 0) lE *= -1;

    p("distance: " << lN + lE);
    // distance: 1565
  }

  void Day12_b()
  {
    static constexpr char c_N = 'N';
    static constexpr char c_E = 'E';
    static constexpr char c_S = 'S';
    static constexpr char c_W = 'W';
    static constexpr char c_L = 'L';
    static constexpr char c_R = 'R';
    static constexpr char c_F = 'F';

    int64_t lrN = 1;
    int64_t lrE = 10;
    int64_t lN = 0;
    int64_t lE = 0;

    u4 dwValue;
    for (char c; cin >> c && c != 'e'; )
    {
      cin >> dwValue;

      switch (c)
      {
      case c_N: lrN += dwValue; break;
      case c_E: lrE += dwValue; break;
      case c_S: lrN -= dwValue; break;
      case c_W: lrE -= dwValue; break;
      case c_L: dwValue = 360 - dwValue;
      case c_R:
      {
        switch (dwValue / 90)
        {
        case 0: break;
        case 1: std::swap(lrN, lrE); lrN *= -1; break;
        case 2: lrN *= -1; lrE *= -1;  break;
        case 3: std::swap(lrN, lrE); lrE *= -1;  break;
        default: p("whooot LR"); return;
        }
      }
      break;
      case c_F:
      {
        lN += lrN * dwValue;
        lE += lrE * dwValue;
      }
      break;
      default: p("whooot c"); return;
      }
    }

    if (lN < 0) lN *= -1;
    if (lE < 0) lE *= -1;

    p("distance2: " << lN + lE);
    // distance2: 78883
  }

  void Day13_a()
  {
    u4 dwMinT = -1;
    u4 dwMinId = 0;

    R(u4, dwTime);
    char c;
    u4 dwId;
    while (1)
    {
      if (!(cin >> dwId))
      {
        cin.clear();
        cin >> c;
      }
      else
      {
        u2 dwT = dwId - dwTime % dwId;

        if (dwT < dwMinT)
        {
          dwMinT = dwT;
          dwMinId = dwId;
        }
      }

      cin >> c;
      if (c == 'e') break;
    }

    p("prod: " << dwMinT * dwMinId);
    // prod: 5946
  }

  void Day13_b()
  {
    {
      R(u4, dwTime);
    }


    char c = 1;
    while (c)
    {
      u8 qwProd = 1;
      u8 qwOff = 0;

      u4 dwId;

      for (u1 id = 0; ; id++)
      {
        if (!(cin >> dwId))
        {
          cin.clear();
          cin >> c;
        }
        else
        {
          qwOff += id;

          bool bFound = false;
          FOR(u4, i, dwId)
          {
            if (qwOff % dwId == 0)
            {
              bFound = true;
              break;
            }

            qwOff += qwProd;
          }

          if (!bFound)
          {
            p("error");
            return;
          }

          qwProd *= dwId;
          qwOff -= id;
        }

        cin >> c;
        if (c == 'r') break;
        if (c == 'e') { c = 0; break; }
      }

      p("offset: " << qwOff);
      // offset: 645338524823718
    }
  }

  void Day14_a()
  {
    str strMask;
    std::map<u8, u8> mMemory;

    char c;
    while (cin >> c && c == 'm')
    {
      cin >> c; // a e
      cin >> c; // s m
      cin >> c; // k [
      if (c == 'k')
      {
        cin >> c; // =
        cin >> strMask;
      }
      else
      {
        u8 qwAddress;
        cin >> qwAddress;

        cin >> c; // ]
        cin >> c; // =

        u8 qwVal;
        cin >> qwVal;
        bitset<36> bs(qwVal);

        FOR(u1, i, 36)
        {
          if (strMask[i] != 'X')
          {
            bs[35 - i] = strMask[i] == '1';
          }
        }

        mMemory.insert_or_assign(qwAddress, bs.to_ullong());
      }
    }

    u8 qwSum = 0;
    for (const auto& rIt : mMemory)
    {
      qwSum += rIt.second;
    }
    p("sum: " << qwSum);
    // sum: 7440382076205
  }

  void Day14_b()
  {
    str strMask;
    std::map<u8, u8> mMemory;

    char c;
    while (cin >> c && c == 'm')
    {
      cin >> c; // a e
      cin >> c; // s m
      cin >> c; // k [
      if (c == 'k')
      {
        cin >> c; // =
        cin >> strMask;
      }
      else
      {
        u8 qwAddress;
        cin >> qwAddress;
        bitset<36> bsIn(qwAddress);

        cin >> c; // ]
        cin >> c; // =

        u8 qwVal;
        cin >> qwVal;

        v<bitset<36>> vAddr;
        vAddr.emplace_back(bsIn);

        FOR(u1, i, 36)
        {
          switch (strMask[35 - i])
          {
          case '0': break;
          case '1':
            if (!bsIn[i])
            {
              FOR(size_t, j, vAddr.size()) vAddr[j].set(i);
            }
            break;
          default:
            size_t total = vAddr.size();
            FOR(size_t, j, total)
            {
              vAddr.emplace_back(vAddr[j]);
              vAddr.back().flip(i);
            }
            break;
          }
        }

        for (auto bs : vAddr)
          mMemory[bs.to_ullong()] = qwVal;
      }
    }

    u8 qwSum = 0;
    for (const auto& rIt : mMemory)
    {
      qwSum += rIt.second;
    }
    p("sum2: " << qwSum);
    // sum2: 4200656704538
  }

  void Day15_a()
  {
    static constexpr u2 wNums = 2020;

    u8 aNums[wNums];

    u2 i = 0;
    while (cin >> aNums[i])
    {
      cin.ignore();
      i++;
    }

    for (; i < wNums; i++)
    {
      for (u2 j = i - 2; 1; j--)
      {
        if (aNums[j] == aNums[i - 1])
        {
          aNums[i] = i - 1 - j;
          break;
        }
        if (j == 0)
        {
          aNums[i] = 0;
          break;
        }
      }
    }

    p("2020: " << aNums[wNums - 1]);
    // 2020: 421
  }

  void Current()
  {
    static constexpr u4 wNums = 30000000;
    v<u4> aNums;
    aNums.resize(wNums);

    u4 i = 0;
    while (cin >> aNums[i])
    {
      cin.ignore();
      i++;
    }

    for (; i < wNums; i++)
    {
      for (u4 j = i - 2; 1; j--)
      {
        if (aNums[j] == aNums[i - 1])
        {
          aNums[i] = i - 1 - j;
          break;
        }
        if (j == 0)
        {
          aNums[i] = 0;
          break;
        }
      }
    }

    p("2020: " << aNums[wNums - 1]);
    // 2020: 421
  }
}

int main()
{
  NAoC2020::Current();

  return 0;
}