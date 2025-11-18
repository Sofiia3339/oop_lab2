<?xml version="1.0" encoding="iso-8859-1"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="html" />
	<xsl:template match="/">
		<html>
			<body>
				<h2>TimeTable</h2>
				<table border="2">
					<tr bgcolor="#fff">
						<th>Full name</th>
						<th>Faculty</th>
						<th>Chair</th>
						<th>Date</th>
						<th>Subject</th>
						<th>Audience</th>
						<th>Students</th>
					</tr>
					<xsl:for-each select="//Person">
						<xsl:for-each select="./Classes/Class">
							<tr>
								<td>
									<xsl:value-of select="concat(./../../Name/FirstName, ' ', ./../../Name/MiddleName, ' ', ./../../Name/LastName)"/>
								</td>
								<td>
									<xsl:value-of select="./../../Faculty"/>
								</td>
								<td>
									<xsl:value-of select="./../../Chair"/>
								</td>
								<td>
									<xsl:value-of select="concat(Date/Day, ' ', Date/Time)"/>
								</td>
								<td>
									<xsl:value-of select="./Subject"/>
								</td>
								<td>
									<xsl:value-of select="./Audience"/>
								</td>
								<td>
									<xsl:for-each select="Students/Student">
										<p><xsl:value-of select="concat(FirstName, ' ', MiddleName, ' ', LastName, ', ', Group)"/></p>
									</xsl:for-each>
								</td>
							</tr>
						</xsl:for-each>
					</xsl:for-each>
				</table>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>
